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
        [HttpPost("{candidateId}/skills/{skillId}")]
        public async Task<IActionResult> AddSkill(string candidateId, string skillId)
        {
            try
            {
                await _service.AddSkillToCandidateAsync(candidateId, skillId);
                return Ok("Skill added.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{candidateId}/skills/{skillId}")]
        public async Task<IActionResult> RemoveSkill(string candidateId, string skillId)
        {
            try
            {
                await _service.DeleteSkillFromCandidateAsync(candidateId, skillId);
                return Ok("Skill deleted.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
