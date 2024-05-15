using AutoMapper;
using CapitalPlacement.Core.DTOs;
using CapitalPlacement.Core.IRepositories;
using CapitalPlacement.Core.IServices;
using CapitalPlacement.Core.Models;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<ApplicationService> _logger;

        public ApplicationService(IApplicationRepository applicationRepository, IMapper mapper, ILogger<ApplicationService> logger)
        {
            _applicationRepository = applicationRepository;
            _mapper = mapper;
            _logger = logger;
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
                _logger.LogError(ex.Message);
            }
        }
    }
}
