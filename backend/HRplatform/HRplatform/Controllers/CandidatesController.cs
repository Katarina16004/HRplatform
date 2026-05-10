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

        // creates candidate
        [HttpPost]
        public async Task<IActionResult> Create(CreateUpdateCandidateRequest request)
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

        // adds skill to candidate by id
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

        // removes skill from candidate by id
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

        // gets all candidates with their skills
        [HttpGet]
        public async Task<IActionResult> GetAllWithSkills()
        {
            try
            {
                var candidates = await _service.GetAllCandidatesWithSkillsAsync();
                return Ok(candidates);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // gets candidate with given id with skills
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdWithSkills(string id)
        {
            try
            {
                var candidate = await _service.GetCandidateWithSkillsByIdAsync(id);
                return Ok(candidate);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // gets candidate with given name with skills
        [HttpGet("byName/{name}")]
        public async Task<IActionResult> GetByNameWithSkills(string name)
        {
            try
            {
                var candidates = await _service.GetCandidatesWithSkillsByNameAsync(name);
                return Ok(candidates);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // updates candidate 
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, CreateUpdateCandidateRequest request)
        {
            try
            {
                bool updated = await _service.UpdateCandidateAsync(id, request);
                if (!updated)
                    return BadRequest("Candidate not found.");

                return Ok("Candidate updated.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // deletes candidate 
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                bool deleted = await _service.DeleteCandidateAsync(id);
                if (!deleted)
                    return BadRequest("Candidate not found.");

                return Ok("Candidate deleted.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // searching candidates by name or/and skills (if has any of skills it will be returned)
        [HttpGet("search")]
        public async Task<IActionResult> Search(string? name, string? skillIds)
        {
            try
            {
                List<string> ids = new List<string>();

                if (!string.IsNullOrWhiteSpace(skillIds))
                {
                    string[] parts = skillIds.Split(',');

                    foreach (var part in parts)
                    {
                        if (!string.IsNullOrWhiteSpace(part))
                            ids.Add(part.Trim());
                    }
                }

                var candidates = await _service.GetCandidatesBySkillsAndNameAsync(name, ids);
                return Ok(candidates);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
