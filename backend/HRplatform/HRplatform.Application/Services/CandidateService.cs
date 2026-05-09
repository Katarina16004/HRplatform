using HRplatform.Application.DTO;
using HRplatform.Application.Interfaces;
using HRplatform.Domain;

namespace HRplatform.Application.Services
{
    public class CandidateService
    {
        private readonly ICandidateRepository _repo;

        public CandidateService(ICandidateRepository repo)
        {
            _repo = repo;
        }

        public async Task<string> AddCandidateAsync(CreateCandidateRequest req)
        {
            if (req == null)
                throw new Exception("Request is required.");

            string fullName = "";
            string email = "";
            string number = "";
            DateOnly? dateOfBirth = null;

            try
            {
                fullName = (req.FullName).Trim();
                email = (req.Email).Trim();
                number = (req.ContactNum).Trim();
                dateOfBirth = req.DateOfBirth;
            }
            catch (Exception ex)
            {
                throw new Exception("Invalid request data: " + ex.Message);
            }

            List<string> errors = new List<string>();

            if (string.IsNullOrWhiteSpace(fullName))
                errors.Add("Full name is required.");
            if (string.IsNullOrWhiteSpace(email))
                errors.Add("Email is required.");
            if (string.IsNullOrWhiteSpace(number))
                errors.Add("Contact number is required.");
            if (req.DateOfBirth == null)
                errors.Add("Date of birth is required.");

            if (errors.Count > 0)
                throw new Exception(string.Join(" ", errors));

            if (await _repo.CandidateExistsByEmailAsync(email))
                throw new Exception("Candidate with this email already exists.");

            Candidate candidate = new Candidate();
            candidate.Id = Guid.NewGuid().ToString();
            candidate.FullName = fullName;
            candidate.DateOfBirth = dateOfBirth;
            candidate.Email = email;
            candidate.ContactNum = number;

            return await _repo.CreateCandidateAsync(candidate);
        }
        public async Task<bool> AddSkillToCandidateAsync(string candidateId, string skillId)
        {
            if (string.IsNullOrWhiteSpace(candidateId))
                throw new Exception("Candidate ID is required.");
            if (string.IsNullOrWhiteSpace(skillId))
                throw new Exception("Skill ID is required.");
            
            if(await _repo.CandidateHasSkillAsync(candidateId, skillId))
                throw new Exception("Candidate already has this skill.");
            return await _repo.AddSkillToCandidateAsync(candidateId, skillId);
        }
        public async Task<bool> DeleteSkillFromCandidateAsync(string candidateId, string skillId)
        {
            if (string.IsNullOrWhiteSpace(candidateId))
                throw new Exception("Candidate ID is required.");
            if (string.IsNullOrWhiteSpace(skillId))
                throw new Exception("Skill ID is required.");

            if(!await _repo.CandidateHasSkillAsync(candidateId, skillId))
                throw new Exception("Candidate does not have this skill.");
            return await _repo.RemoveSkillFromCandidateAsync(candidateId, skillId);
        }
    }
}
