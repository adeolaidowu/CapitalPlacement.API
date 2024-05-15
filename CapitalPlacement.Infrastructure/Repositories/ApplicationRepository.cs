using CapitalPlacement.Core.IRepositories;
using CapitalPlacement.Core.Models;
using CapitalPlacement.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapitalPlacement.Infrastructure.Repositories
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly AppDbContext _ctx;

        public ApplicationRepository(AppDbContext ctx)
        {
            _ctx = ctx;
        }
        public async Task SubmitCandidateApplicationAsync(CandidateApplication candidateApplicationDoc)
        {
            await _ctx.CandidatesApplications.AddAsync(candidateApplicationDoc);
            await _ctx.SaveChangesAsync();

        }
    }
}
