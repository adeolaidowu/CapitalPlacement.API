using CapitalPlacement.Core.DTOs;
using CapitalPlacement.Core.IServices;
using CapitalPlacement.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CapitalPlacement.API.Controllers
{
    [Route("api/application")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationService _applicationService;

        public ApplicationController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }
        [HttpPost("submit")]
        public async Task<IActionResult> SubmitApplication(CandidateApplicationDto candidate)
        {
            if (candidate == null)
            {
                return BadRequest("Candidate data is null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _applicationService.SubmitApplicationAsync(candidate);
                return Ok("Application submitted successfully.");
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Something went wrong" });
            }

            
        }
    }
}
