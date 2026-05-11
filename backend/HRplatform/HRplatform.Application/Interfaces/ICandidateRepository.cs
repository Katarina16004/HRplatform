using HRplatform.Domain;

namespace HRplatform.Application.Interfaces
{
    public interface ICandidateRepository
    {
        Task<string> CreateCandidateAsync(Candidate candidate);
        Task<bool> CandidateExistsByEmailAsync(string email); // email is unique for each candidate
        Task<bool> CandidateExistsByEmailExceptThisIdAsync(string email, string id); // for update operation, to check if another candidate has the same email
        Task<bool> CandidateExistsByIdAsync(string id);
        Task<bool> AddSkillToCandidateAsync(string candidateId, string skillId);
        Task<bool> RemoveSkillFromCandidateAsync(string candidateId, string skillId);
        Task<bool> CandidateHasSkillAsync(string candidateId, string skillId);
        Task<List<Candidate>> GetAllCandidatesWithSkillsAsync();
        Task<Candidate?> GetCandidateWithSkillsByIdAsync(string id);
        Task<List<Candidate>> GetCandidatesWithSkillsByNameAsync(string name); // it can be more than one candidate with the same name
        Task<List<Candidate>> GetCandidatesBySkillsAndNameAsync(string? name, List<string>? skillName); // to find candidates with that skills and/or name
        Task<bool> DeleteCandidateAsync(string id);
        Task<bool> UpdateCandidateAsync(Candidate candidate);

    }
}
