using HRplatform.Application.DTO;
using HRplatform.Application.Interfaces;
using HRplatform.Domain;

namespace HRplatform.Application.Services
{
    public class SkillService
    {
        private readonly ISkillRepository _repo;

        public SkillService(ISkillRepository repo)
        {
            _repo = repo;
        }

        public async Task<string> AddSkillAsync(CreateSkillRequest req)
        {
            string name = "";
            if (req.Name == null)
                throw new Exception("Request is required.");
            else
                name = req.Name.Trim();

            string nameLower = name.ToLowerInvariant();

            if (await _repo.SkillExistsByNameAsync(nameLower))
                throw new Exception("Skill already exists.");

            var skill = new Skill
            {
                Id = Guid.NewGuid().ToString(),
                Name = name
            };

            return await _repo.CreateSkillAsync(skill);
        }
        public async Task<List<Skill>> GetAllSkillsAsync()
        {
            return await _repo.GetAllSkillsAsync();
        }
        public async Task<Skill?> GetSkillByIdAsync(string id)
        {
            if(string.IsNullOrWhiteSpace(id))
                throw new Exception("Id is required.");
            Skill? skill = await _repo.GetSkillByIdAsync(id);
            if (skill == null)
                throw new Exception("Skill not found.");

            return skill;
        }
    }
}