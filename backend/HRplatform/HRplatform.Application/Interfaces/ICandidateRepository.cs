using HRplatform.Domain;

namespace HRplatform.Application.Interfaces
{
    public interface ICandidateRepository
    {
        Task<string> CreateCandidateAsync(Candidate candidate);
        Task<bool> CandidateExistsByEmailAsync(string email); // email is unique for each candidate
        Task<bool> AddSkillToCandidateAsync(string candidateId, string skillId);
        Task<bool> RemoveSkillFromCandidateAsync(string candidateId, string skillId);
        Task<bool> CandidateHasSkillAsync(string candidateId, string skillId);
    }
}
