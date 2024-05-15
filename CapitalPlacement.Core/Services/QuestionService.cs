using AutoMapper;
using CapitalPlacement.Core.DTOs;
using CapitalPlacement.Core.IServices;
using CapitalPlacement.Core.Models;
using CapitalPlacement.Core.Response;
using CapitalPlacement.Infrastructure.IRepositories;
using Microsoft.Extensions.Logging;

namespace CapitalPlacement.Core.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<QuestionService> _logger;

        public QuestionService(IQuestionRepository questionRepository, IMapper mapper, ILogger<QuestionService> logger)
        {
            _questionRepository = questionRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<ApiResponse<Question>> CreateQuestionAsync(QuestionDto question)
        {
            var response = new ApiResponse<Question>();

            try
            {
                var validationResult = ValidateQuestionDto(question);
                if (!validationResult.Success)
                {
                    return validationResult;
                }

                var newQuestion = _mapper.Map<Question>(question);
                newQuestion.Id = Guid.NewGuid().ToString();

                await _questionRepository.CreateAsync(newQuestion);

                response.Success = true;
                response.Message = "A new question has successfully been created";
                response.Data = newQuestion;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating question");

                // Return failure response
                response.Success = false;
                response.Message = "Something went wrong";
                return response;
            }
        }

        public async Task<ApiResponse<GetQuestionDto>> GetQuestionByIdAsync(string questionId)
        {
            var response = new ApiResponse<GetQuestionDto>();
            try
            {
                var question = await _questionRepository.GetByIdAsync(questionId);
                var returnedQuestion = _mapper.Map<GetQuestionDto>(question);
                if (question != null)
                {
                    //question.Type = (QuestionType)question.Type;
                    response.Data = returnedQuestion;
                    return response;
                }

                response.Success = false;
                response.Data = returnedQuestion;
                response.Message = "Not Found";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response.Success = false;
                response.Data = default;
                response.Message = "Internal Server Error";
                return response;
            }
        }

        public async Task<ApiResponse<IEnumerable<GetQuestionDto>>> GetQuestionsByTypeAsync(QuestionType questionType)
        {
            var response = new ApiResponse<IEnumerable<GetQuestionDto>>();
            try
            {
                var questions = await _questionRepository.GetAllByTypeAsync(questionType);
                var questionsDto = _mapper.Map<IEnumerable<GetQuestionDto>>(questions);
                response.Data = questionsDto;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                response.Success = false;
                return response;
            }
        }

        public async Task<bool> UpdateQuestionAsync(string questionId, QuestionDto question)
        {
            try
            {
                var questionFromDb = await _questionRepository.GetByIdAsync(questionId);
                if (questionFromDb == null) return false;

                if (questionFromDb.Type != question.Type)
                {
                    if (!ValidateAndUpdateForTypeChange(questionFromDb, question))
                    {
                        return false;
                    }
                }
                else
                {
                    UpdateForSameType(questionFromDb, question);
                }

                questionFromDb.Text = question.Text;
                questionFromDb.Type = question.Type;

                var successfullyUpdated = await _questionRepository.UpdateAsync(questionFromDb);
                return successfullyUpdated;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating question with ID {QuestionId}", questionId);
                return false;
            }
        }

        private ApiResponse<Question> ValidateQuestionDto(QuestionDto question)
        {
            var response = new ApiResponse<Question>();

            if (question == null)
            {
                response.Success = false;
                response.Message = "Question cannot be null";
                return response;
            }

            if (question.Type == QuestionType.MultipleChoice || question.Type == QuestionType.Dropdown)
            {
                if (question.Options?.Count == 0)
                {
                    response.Success = false;
                    response.Message = "You need to add options for this question";
                    return response;
                }

                if (question.Type == QuestionType.MultipleChoice && question.MaxChoiceAllowed <= 0)
                {
                    response.Success = false;
                    response.Message = "You need to specify the maximum choices allowed for this question type";
                    return response;
                }
            }

            if ((question.Type != QuestionType.MultipleChoice && question.Type != QuestionType.Dropdown) && question.Options?.Count > 0)
            {
                response.Success = false;
                response.Message = "You cannot add options for question types that are not Multiple Choice or Dropdown";
                return response;
            }

            if ((question.Type != QuestionType.MultipleChoice && question.Type != QuestionType.Dropdown) && question.EnableOtherOption)
            {
                response.Success = false;
                response.Message = "Enable other options is only valid for Multiple Choice and Dropdown question types";
                return response;
            }

            if (question.Type != QuestionType.MultipleChoice && question.MaxChoiceAllowed != 0)
            {
                response.Success = false;
                response.Message = "You cannot specify maximum choice allowed for this question type";
                return response;
            }

            response.Success = true;
            return response;
        }

        private bool ValidateAndUpdateForTypeChange(Question questionFromDb, QuestionDto question)
        {
            switch (question.Type)
            {
                case QuestionType.MultipleChoice:
                    if (question.Options.Count < 1 || question.MaxChoiceAllowed <= 1)
                    {
                        return false;
                    }
                    questionFromDb.Options = question.Options;
                    questionFromDb.MaxChoiceAllowed = question.MaxChoiceAllowed;
                    questionFromDb.EnableOtherOption = question.EnableOtherOption;
                    break;

                case QuestionType.Dropdown:
                    if (question.Options.Count < 1)
                    {
                        return false;
                    }
                    questionFromDb.Options = question.Options;
                    questionFromDb.EnableOtherOption = question.EnableOtherOption;
                    break;

                case QuestionType.Number:
                case QuestionType.YesOrNo:
                case QuestionType.Date:
                case QuestionType.Paragraph:
                    questionFromDb.Options = null;
                    questionFromDb.EnableOtherOption = false;
                    questionFromDb.MaxChoiceAllowed = 0;
                    break;

                default:
                    return false;
            }
            return true;
        }

        private void UpdateForSameType(Question questionFromDb, QuestionDto question)
{
    if (question.Type == QuestionType.MultipleChoice)
    {
        questionFromDb.Options = question.Options.Count > 0 ? question.Options : questionFromDb.Options;
        questionFromDb.MaxChoiceAllowed = question.MaxChoiceAllowed > 1 ? question.MaxChoiceAllowed : questionFromDb.MaxChoiceAllowed;
        questionFromDb.EnableOtherOption = question.EnableOtherOption;
    }
    else if (question.Type == QuestionType.Dropdown)
    {
        questionFromDb.Options = question.Options.Count > 0 ? question.Options : questionFromDb.Options;
        questionFromDb.EnableOtherOption = question.EnableOtherOption;
    }
}
    }
}
