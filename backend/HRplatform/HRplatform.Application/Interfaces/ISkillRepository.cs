using HRplatform.Domain;

namespace HRplatform.Application.Interfaces
{
    public interface ISkillRepository
    {
        Task<bool> SkillExistsByNameAsync(string name);
        Task<string> CreateSkillAsync(Skill skill);
        Task<List<Skill>> GetAllSkillsAsync();
        Task<Skill?> GetSkillByIdAsync(string id);
    }
}
