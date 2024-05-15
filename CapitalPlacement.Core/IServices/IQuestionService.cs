using CapitalPlacement.Core.DTOs;
using CapitalPlacement.Core.Models;
using CapitalPlacement.Core.Response;

namespace CapitalPlacement.Core.IServices
{
    public interface IQuestionService
    {
        Task<ApiResponse<Question>> CreateQuestionAsync(QuestionDto question);
        Task<bool> UpdateQuestionAsync(string questionId, QuestionDto question);
        Task<ApiResponse<IEnumerable<GetQuestionDto>>> GetQuestionsByTypeAsync(QuestionType questionType);

        Task<ApiResponse<GetQuestionDto>> GetQuestionByIdAsync(string questionId);
    }
}
