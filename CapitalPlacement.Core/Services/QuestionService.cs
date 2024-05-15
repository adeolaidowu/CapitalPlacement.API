using AutoMapper;
using CapitalPlacement.Core.DTOs;
using CapitalPlacement.Core.IServices;
using CapitalPlacement.Core.Models;
using CapitalPlacement.Core.Response;
using CapitalPlacement.Infrastructure.IRepositories;

namespace CapitalPlacement.Core.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IMapper _mapper;

        public QuestionService(IQuestionRepository questionRepository, IMapper mapper)
        {
            _questionRepository = questionRepository;
            _mapper = mapper;
        }
        public async Task<ApiResponse<Question>> CreateQuestionAsync(CreateQuestionDto question)
        {
            var response = new ApiResponse<Question>();

            try
            {
                var newQuestion = _mapper.Map<Question>(question);
                newQuestion.Id = Guid.NewGuid().ToString();
                if (question.Type == QuestionType.MultipleChoice || question.Type == QuestionType.Dropdown)
                {
                    if (question.Options?.Count == 0)
                    {
                        response.Success = false;
                        response.Message = "You need to add options for this question";
                        return response;
                    }
                    
                }
                if(question.Type == QuestionType.MultipleChoice)
                {
                    if (question.MaxChoiceAllowed <= 0)
                    {
                        response.Success = false;
                        response.Message = "You need to specify the maximum choices allowed for this question type";
                        return response;
                    }
                }

                if (question.Type != QuestionType.MultipleChoice && question.Type != QuestionType.Dropdown)
                {
                    if (question.Options?.Count > 0)
                    {
                        response.Success = false;
                        response.Message = "You cannot add options for question types that are not Multiple Choice or Dropdown";
                        return response;
                    }
                }
                if (question.Type != QuestionType.MultipleChoice && question.Type != QuestionType.Dropdown)
                {
                    if (question.EnableOtherOption)
                    {
                        response.Success = false;
                        response.Message = "Enable other options is only valid for Multiple Choice and Dropdown question types";
                        return response;
                    }
                }
                if (question.Type != QuestionType.MultipleChoice)
                {
                    if (question.MaxChoiceAllowed != 0)
                    {
                        response.Success = false;
                        response.Message = "You cannot specify maximum choice allowed for this question type";
                        return response;
                    }
                }
                
                await _questionRepository.CreateAsync(newQuestion);
                response.Message = "A new question has successfully been created";
                response.Data = newQuestion;
                return response;
            }
            catch (Exception ex)
            {
                // log error
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
                // log exception
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
                // log ex
                response.Success = false;
                return response;
            }
        }

        public async Task<bool> UpdateQuestionAsync(string questionId, CreateQuestionDto question)
        {
            try
            {
                var questionFromDb = await _questionRepository.GetByIdAsync(questionId);
                if (questionFromDb == null) return false;
                if(questionFromDb.Type != question.Type)
                {
                    if (question.Type == QuestionType.MultipleChoice)
                    {
                        if (question.Options.Count < 1 || question.MaxChoiceAllowed <= 1) return false;
                        questionFromDb.Options = question.Options;
                        questionFromDb.MaxChoiceAllowed = question.MaxChoiceAllowed;
                        questionFromDb.EnableOtherOption = question.EnableOtherOption;
                    }
                    if (question.Type == QuestionType.Dropdown)
                    {
                        if (question.Options.Count < 1) return false;
                        questionFromDb.Options = question.Options;
                        questionFromDb.EnableOtherOption = question.EnableOtherOption;
                    }
                    if(question.Type == QuestionType.Number || question.Type == QuestionType.YesOrNo || question.Type == QuestionType.Date || question.Type == QuestionType.Paragraph)
                    {
                        questionFromDb.Options = null;
                        questionFromDb.EnableOtherOption = false;
                        questionFromDb.MaxChoiceAllowed = 0;
                    }
                }
                else if(questionFromDb.Type == question.Type)
                {
                    if(question.Type == QuestionType.MultipleChoice)
                    {
                        questionFromDb.Options = question.Options.Count > 0 ? question.Options : questionFromDb.Options;
                        questionFromDb.MaxChoiceAllowed = question.MaxChoiceAllowed > 1 ? question.MaxChoiceAllowed : questionFromDb.MaxChoiceAllowed;
                        questionFromDb.EnableOtherOption = question.EnableOtherOption;
                    }
                    else if(question.Type == QuestionType.Dropdown)
                    {
                        questionFromDb.Options = question.Options.Count > 0 ? question.Options : questionFromDb.Options;
                        questionFromDb.EnableOtherOption = question.EnableOtherOption;
                    }
                }
                questionFromDb.Text = question.Text;
                questionFromDb.Type = question.Type;
                var successfullyUpdated = await _questionRepository.UpdateAsync(questionFromDb);
                return successfullyUpdated;
            }
            catch (Exception ex)
            {
                // log error
                return false;
            }
        }
    }
}
