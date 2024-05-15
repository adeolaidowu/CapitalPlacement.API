using CapitalPlacement.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapitalPlacement.Core.IRepositories
{
    public interface IApplicationRepository
    {
        Task SubmitCandidateApplicationAsync(CandidateApplication candidateApplicationDoc);
    }
}
