using AutoMapper;
using CapitalPlacement.Core.DTOs;
using CapitalPlacement.Core.IRepositories;
using CapitalPlacement.Core.IServices;
using CapitalPlacement.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapitalPlacement.Core.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IMapper _mapper;

        public ApplicationService(IApplicationRepository applicationRepository, IMapper mapper)
        {
            _applicationRepository = applicationRepository;
            _mapper = mapper;
        }
        public async Task SubmitApplicationAsync(CandidateApplicationDto candidateApplication)
        {
            try
            {
                var application = _mapper.Map<CandidateApplication>(candidateApplication);
                await _applicationRepository.SubmitCandidateApplicationAsync(application);
            }
            catch (Exception ex)
            {
                // log ex
                throw;
            }
        }
    }
}
