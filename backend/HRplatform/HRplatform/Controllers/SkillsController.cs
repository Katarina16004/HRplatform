using HRplatform.Application.DTO;
using HRplatform.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRplatform.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillsController : ControllerBase
    {
        private readonly SkillService _service;
        public SkillsController(SkillService service)
        {
            _service = service;
        }

        // creates skill
        [HttpPost]
        public async Task<IActionResult> Create(CreateSkillRequest request)
        {
            try
            {
                string id = await _service.AddSkillAsync(request);
                return Ok(new { id = id });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // gets all skills
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var skills = await _service.GetAllSkillsAsync();
                return Ok(skills);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // gets skill by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var skill = await _service.GetSkillByIdAsync(id);
                return Ok(skill);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //get skill by name (name is unique) (c = C)
        [HttpGet("byName/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            try
            {
                var skill = await _service.GetSkillByNameAsync(name);
                return Ok(skill);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // deletes skill 
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                bool deleted = await _service.DeleteSkillAsync(id);
                if (deleted)
                    return Ok(new { message = "Skill deleted successfully." });
                else
                    return NotFound(new { message = "Skill not found." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
