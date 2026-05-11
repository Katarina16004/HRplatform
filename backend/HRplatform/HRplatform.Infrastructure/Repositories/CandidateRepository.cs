using HRplatform.Application.Interfaces;
using HRplatform.Domain;
using HRplatform.Infrastructure.Db;
using MySqlConnector;

namespace HRplatform.Infrastructure.Repositories
{
    public class CandidateRepository:ICandidateRepository
    {
        private readonly MySqlConnectionFactory _factory;

        // dependency injection
        public CandidateRepository(MySqlConnectionFactory factory)
        {
            _factory = factory;
        }

        // adds new candidate to the database and returns the id of the created candidate
        public async Task<string> CreateCandidateAsync(Domain.Candidate candidate)
        {
            string sql = "INSERT INTO Candidate (id,full_name, date_of_birth, email, contact_num) VALUES (@id, @fullName, @dateOfBirth, @email, @contactNum);";
            using (MySqlConnection conn = _factory.Create()) // creates connection to the database
            {
                await conn.OpenAsync(); // opens the connection to the database
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))  // for sql command
                {
                    cmd.Parameters.AddWithValue("@id", candidate.Id);  // adds parameters
                    cmd.Parameters.AddWithValue("@fullName", candidate.FullName);
                    cmd.Parameters.AddWithValue("@dateOfBirth", candidate.DateOfBirth);
                    cmd.Parameters.AddWithValue("@email", candidate.Email);
                    cmd.Parameters.AddWithValue("@contactNum", candidate.ContactNum);

                    await cmd.ExecuteNonQueryAsync();  // executes the command
                    return candidate.Id;
                }
            }
        }

        // checks if candidate with the given email exists in the database
        public async Task<bool> CandidateExistsByEmailAsync(string email)
        {
            string sql = "SELECT 1 FROM Candidate WHERE email = @email;";
            using (MySqlConnection conn = _factory.Create())
            {
                await conn.OpenAsync();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@email", email);

                    object? result = await cmd.ExecuteScalarAsync(); // returns first column of the first row or null
                    return result != null;
                }
            }
        }

        // checks if candidate with the given email exists in the database except the itself (for updating)
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

        // checks if candidate with the given id exists in the database
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

        // checks if candidate has the skill with the given skill id
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
        
        // adds a skill to a candidate
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

        // removes a skill from a candidate
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

        // all skills of a candidate with the given candidate id
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

        // gets all candidates with their skills
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

        // gets a candidate with the given id with skills
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

        // gets candidates with the given name with skills
        public async Task<List<Candidate>> GetCandidatesWithSkillsByNameAsync(string name)
        {
            // personally, when i search in real life, i rarely use correct capitalization (same for name of skills, C is same like c for me)
            // database supports case insensitive 

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

        // deletes a candidate with the given id from the database
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

        // updates a candidate
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

        // searches candidates by name and skills
        // if name is empty searches only by skills, if skills is empty searches only by name
        // in service layer, both name and skills can`t be empty
        public async Task<List<Candidate>> GetCandidatesBySkillsAndNameAsync(string? name, List<string>? skillName)
        {
            string nameValue="";
            bool hasName = false; 
            bool hasSkills = false;
            if (!string.IsNullOrWhiteSpace(name))
            {
                hasName = true;
                nameValue = name.Trim();
            }

            List<string> ids = new List<string>();

            if (skillName != null)
            {
                foreach (var skill in skillName)
                { 
                    string? v = skill;
                    if (string.IsNullOrWhiteSpace(v))
                        continue;
                    ids.Add(v.Trim());
                    hasSkills = true;
                }
            }

            if (!hasName && !hasSkills)
                return new List<Candidate>(); // in service layer, we will write error message

            string sql = "SELECT DISTINCT c.id, c.full_name, c.date_of_birth, c.email, c.contact_num FROM Candidate c ";

            if (hasSkills) // if we have skills, we are joining tables
            {
                sql += "INNER JOIN candidate_skills cs ON cs.candidate_id = c.id ";
                sql += "INNER JOIN skill s ON s.id = cs.skill_id ";  // we need for skill name
            }

            sql += "WHERE 1=1 "; // if we have name or skills, we will add conditions, if not, this condition will be always true

            if (hasName) // if we have name, we are adding condition for name
                sql += "AND c.full_name LIKE @name ";

            if (hasSkills)
            {
                List<string> parameters = new List<string>();

                for (int i = 0; i < ids.Count; i++)
                {
                    parameters.Add("@s" + i); // we are adding parameters for skills
                }

                string inList = string.Join(", ", parameters);

                sql += $"AND s.name IN ({inList}) \n"; // any skill can match
            }

            List<Candidate> candidates = new List<Candidate>();

            using (MySqlConnection conn = _factory.Create())
            {
                await conn.OpenAsync();

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    if (hasName)
                        cmd.Parameters.AddWithValue("@name", nameValue);

                    if (hasSkills)
                    {
                        for (int i = 0; i < ids.Count; i++)
                            cmd.Parameters.AddWithValue("@s" + i, ids[i]);
                    }

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
                    c.Skills = await GetSkillsForCandidateAsync(conn, c.Id);
            }

            return candidates;
        }
    }
}
