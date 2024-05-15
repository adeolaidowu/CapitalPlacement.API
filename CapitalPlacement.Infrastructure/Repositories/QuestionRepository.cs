using CapitalPlacement.Core.DTOs;
using CapitalPlacement.Core.Models;
using CapitalPlacement.Infrastructure.Data;
using CapitalPlacement.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapitalPlacement.Infrastructure.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly AppDbContext _ctx;

        public QuestionRepository(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<Question> CreateAsync(Question questionDoc)
        {
            await _ctx.Questions.AddAsync(questionDoc);
            await _ctx.SaveChangesAsync();
            return questionDoc;
        }

        public async Task<Question> GetByIdAsync(string id)
        {
            var question = await _ctx.Questions.FindAsync(id);
            return question;
        }

        public async Task<IEnumerable<Question>> GetAllByTypeAsync(QuestionType questionType)
        {
            var questions = await _ctx.Questions.Where(q => q.Type == questionType).AsNoTracking().ToListAsync();
            return questions;
        }

        public async Task<bool> UpdateAsync(Question updatedQuestionDoc)
        {
            _ctx.Update(updatedQuestionDoc);
            await _ctx.SaveChangesAsync();
            return true;
        }
    }
}
