using CapitalPlacement.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapitalPlacement.Core.IServices
{
    public interface IApplicationService
    {
        Task SubmitApplicationAsync(CandidateApplicationDto candidateApplication);
    }
}
