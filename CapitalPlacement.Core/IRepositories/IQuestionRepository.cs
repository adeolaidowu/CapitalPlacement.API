using CapitalPlacement.Core.DTOs;
using CapitalPlacement.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapitalPlacement.Infrastructure.IRepositories
{
    public interface IQuestionRepository
    {
        Task<Question> GetByIdAsync(string id);
        Task<IEnumerable<Question>> GetAllByTypeAsync(QuestionType questionType);
        Task<Question> CreateAsync(Question questionDoc);
        Task<bool> UpdateAsync(Question updatedQuestionDoc);
    }
}
