using HRplatform.Application.DTO;
using HRplatform.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRplatform.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidatesController : ControllerBase
    {
        private readonly CandidateService _service;
        public CandidatesController(CandidateService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCandidateRequest request)
        {
            try
            {
                string id = await _service.AddCandidateAsync(request);
                return Ok(new { id = id });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
