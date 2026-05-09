using HRplatform.Application.Interfaces;
using HRplatform.Domain;
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
        public async Task<bool> CandidateExistsByEmailExceptThisIdAsync(string email, string id)
        {
            string sql = "SELECT 1 FROM Candidate WHERE email = @email AND id != @id;";
            using (MySqlConnection conn = _factory.Create())
            {
                await conn.OpenAsync();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@email", email);

                    object? result = await cmd.ExecuteScalarAsync();
                    return result != null;
                }
            }
        }
        public async Task<bool> CandidateExistsByIdAsync(string id)
        {
            string sql = "SELECT 1 FROM Candidate WHERE id = @id;";
            using (MySqlConnection conn = _factory.Create())
            {
                await conn.OpenAsync();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);

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

        private async Task<List<Skill>> GetSkillsForCandidateAsync(MySqlConnection conn, string candidateId)
        {
            string sql = "SELECT s.id, s.name FROM candidate_skills cs INNER JOIN skill s ON s.id = cs.skill_id WHERE cs.candidate_id = @candidate_id;";

            List<Skill> skills = new List<Skill>();

            using (MySqlCommand cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@candidate_id", candidateId);
                using (MySqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        Skill s = new Skill();
                        s.Id = reader["id"].ToString();
                        s.Name = reader.GetString("name");
                        skills.Add(s);
                    }
                }
            }

            return skills;
        }
        public async Task<List<Candidate>> GetAllCandidatesWithSkillsAsync()
        {
            string sql = "SELECT id, full_name, date_of_birth, email, contact_num FROM Candidate;";
            List<Candidate> candidates = new List<Candidate>();

            using (MySqlConnection conn = _factory.Create())
            {
                await conn.OpenAsync();

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    using (MySqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Candidate c = new Candidate();
                            c.Id = reader["id"].ToString();
                            c.FullName = reader.GetString("full_name");
                            c.DateOfBirth = DateOnly.FromDateTime(reader.GetDateTime("date_of_birth"));
                            c.Email = reader.GetString("email");
                            c.ContactNum = reader.GetString("contact_num");
                            candidates.Add(c);
                        }
                    }
                }

                foreach (Candidate c in candidates)
                {
                    c.Skills = await GetSkillsForCandidateAsync(conn, c.Id);
                }
            }

            return candidates;
        }

        public async Task<Candidate?> GetCandidateWithSkillsByIdAsync(string id)
        {
            string sql = "SELECT * FROM Candidate WHERE id = @id;";

            using (MySqlConnection conn = _factory.Create())
            {
                await conn.OpenAsync();
                Candidate? c = null;

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (MySqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            c = new Candidate();
                            c.Id = reader["id"].ToString();
                            c.FullName = reader.GetString("full_name");
                            c.DateOfBirth = DateOnly.FromDateTime(reader.GetDateTime("date_of_birth"));
                            c.Email = reader.GetString("email");
                            c.ContactNum = reader.GetString("contact_num");
                        }
                    }
                }

                if (c == null)
                    return null;

                c.Skills = await GetSkillsForCandidateAsync(conn, c.Id);
                return c;
            }
        }

        public async Task<List<Candidate>> GetCandidatesWithSkillsByNameAsync(string name)
        {
            string sql = "SELECT * FROM Candidate WHERE full_name = @name;";
            List<Candidate> candidates = new List<Candidate>(); 
            using (MySqlConnection conn = _factory.Create())
            {
                await conn.OpenAsync();
                Candidate? c = null;

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    using (MySqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            c = new Candidate();
                            c.Id = reader["id"].ToString();
                            c.FullName = reader.GetString("full_name");
                            c.DateOfBirth = DateOnly.FromDateTime(reader.GetDateTime("date_of_birth"));
                            c.Email = reader.GetString("email");
                            c.ContactNum = reader.GetString("contact_num");
                            candidates.Add(c);
                        }
                    }
                }

                foreach (Candidate cand in candidates)
                {
                    cand.Skills = await GetSkillsForCandidateAsync(conn, cand.Id);
                }
                return candidates;
            }
        }

        public async Task<bool> DeleteCandidateAsync(string id)
        {
            string sql = "DELETE FROM Candidate WHERE id = @id;";
            using (MySqlConnection conn = _factory.Create())
            {
                await conn.OpenAsync();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    int rows = await cmd.ExecuteNonQueryAsync();
                    return rows > 0;
                }
            }
        }

        public async Task<bool> UpdateCandidateAsync(Candidate candidate)
        {
            string sql = "UPDATE Candidate SET full_name = @full_name, date_of_birth = @date_of_birth, email = @email, contact_num = @contact_num WHERE id = @id;";
            using (MySqlConnection conn = _factory.Create())
            {
                await conn.OpenAsync();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", candidate.Id);
                    cmd.Parameters.AddWithValue("@full_name", candidate.FullName);
                    cmd.Parameters.AddWithValue("@date_of_birth", candidate.DateOfBirth);
                    cmd.Parameters.AddWithValue("@email", candidate.Email);
                    cmd.Parameters.AddWithValue("@contact_num", candidate.ContactNum);

                    int rows = await cmd.ExecuteNonQueryAsync();
                    return rows > 0;
                }
            }
        }
    }
}
