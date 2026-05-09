using HRplatform.Application.Interfaces;
using HRplatform.Infrastructure.Db;
using MySqlConnector;

namespace HRplatform.Infrastructure.Repositories
{
    public class CandidateRepository:ICandidateRepository
    {
        private readonly MySqlConnectionFactory _factory;

        public CandidateRepository(MySqlConnectionFactory factory)
        {
            _factory = factory;
        }
        public async Task<string> CreateCandidateAsync(Domain.Candidate candidate)
        {
            string sql = "INSERT INTO Candidate (id,full_name, date_of_birth, email, contact_num) VALUES (@id, @fullName, @dateOfBirth, @email, @contactNum);";
            using (MySqlConnection conn = _factory.Create())
            {
                await conn.OpenAsync();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", candidate.Id);
                    cmd.Parameters.AddWithValue("@fullName", candidate.FullName);
                    cmd.Parameters.AddWithValue("@dateOfBirth", candidate.DateOfBirth);
                    cmd.Parameters.AddWithValue("@email", candidate.Email);
                    cmd.Parameters.AddWithValue("@contactNum", candidate.ContactNum);

                    await cmd.ExecuteNonQueryAsync();
                    return candidate.Id;
                }
            }
        }

        public async Task<bool> CandidateExistsByEmailAsync(string email)
        {
            string sql = "SELECT 1 FROM Candidate WHERE email = @email;";
            using (MySqlConnection conn = _factory.Create())
            {
                await conn.OpenAsync();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@email", email);

                    object? result = await cmd.ExecuteScalarAsync();
                    return result != null;
                }
            }
        }

        public async Task<bool> CandidateHasSkillAsync(string candidateId, string skillId)
        {
            string sql = "SELECT 1 FROM candidate_skills WHERE candidate_id = @candidate_id AND skill_id = @skill_id;";
            using (MySqlConnection conn = _factory.Create())
            {
                await conn.OpenAsync();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@candidate_id", candidateId);
                    cmd.Parameters.AddWithValue("@skill_id", skillId);
                    object? result = await cmd.ExecuteScalarAsync();
                    return result != null;
                }
            }
        }

        public async Task<bool> AddSkillToCandidateAsync(string candidateId, string skillId)
        {
            string sql = "INSERT INTO candidate_skills (candidate_id, skill_id) VALUES (@candidate_id, @skill_id);";
            using (MySqlConnection conn = _factory.Create())
            {
                await conn.OpenAsync();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@candidate_id", candidateId);
                    cmd.Parameters.AddWithValue("@skill_id", skillId);

                    int rows = await cmd.ExecuteNonQueryAsync();
                    return rows > 0;
                }
            }
        }

        public async Task<bool> RemoveSkillFromCandidateAsync(string candidateId, string skillId)
        {
            string sql = "DELETE FROM candidate_skills WHERE candidate_id = @candidate_id AND skill_id = @skill_id;";

            using (MySqlConnection conn = _factory.Create())
            {
                await conn.OpenAsync();

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@candidate_id", candidateId);
                    cmd.Parameters.AddWithValue("@skill_id", skillId);

                    int rows = await cmd.ExecuteNonQueryAsync();
                    return rows > 0;
                }
            }
        }
    }
}
