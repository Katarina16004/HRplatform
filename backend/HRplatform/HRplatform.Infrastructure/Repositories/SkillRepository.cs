using HRplatform.Application.Interfaces;
using HRplatform.Domain;
using HRplatform.Infrastructure.Db;
using MySqlConnector;

namespace HRplatform.Infrastructure.Repositories
{
    public class SkillRepository : ISkillRepository
    {
        private readonly MySqlConnectionFactory _factory;

        // dependency injection
        public SkillRepository(MySqlConnectionFactory factory)
        {
            _factory = factory;
        }

        // adds new skill to the database and returns the id of the created skill
        public async Task<string> CreateSkillAsync(Domain.Skill skill)
        {
            string sql = "INSERT INTO Skill (id, name) VALUES (@id, @name);";
            using (MySqlConnection conn = _factory.Create()) // creates connection to the database
            {
                await conn.OpenAsync(); // opens the connection to the database
                using (MySqlCommand cmd = new MySqlCommand(sql, conn)) // for sql command
                {
                    cmd.Parameters.AddWithValue("@id", skill.Id);  // adds parameters
                    cmd.Parameters.AddWithValue("@name", skill.Name);

                    await cmd.ExecuteNonQueryAsync(); // executes the command
                    return skill.Id;
                }
            }
        }

        // gets skill with the given name
        public async Task<bool> SkillExistsByNameAsync(string name)
        {
            string sql = "SELECT 1 FROM Skill WHERE name = @name;";
            using (MySqlConnection conn = _factory.Create())
            {
                await conn.OpenAsync();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@name", name);

                    object? result = await cmd.ExecuteScalarAsync(); // returns first column of the first row or null
                    return result != null;
                }
            }
        }

        // gets all skills from database
        public async Task<List<Skill>> GetAllSkillsAsync()
        {
            string sql = "SELECT * FROM Skill;";
            List<Skill> skills = new List<Skill>();
            using (MySqlConnection conn = _factory.Create())
            {
                await conn.OpenAsync();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
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
        }

        // gets a skill with the given id
        public async Task<Skill?> GetSkillByIdAsync(string id)
        {
            string sql = "SELECT * FROM Skill WHERE id = @id;";
            using (MySqlConnection conn = _factory.Create())
            {
                await conn.OpenAsync();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (MySqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            Skill s = new Skill();
                            s.Id = reader["id"].ToString();
                            s.Name = reader.GetString("name");
                            return s;
                        }
                    }
                }
            }
            return null;
        }

        // gets a skill with the given name 
        public async Task<Skill?> GetSkillByNameAsync(string name)
        {
            // personally, when i search in real life, i rarely use correct capitalization (same for name of candidate)
            // database supports case insensitive 

            string sql = "SELECT * FROM Skill WHERE name = @name;";
            using (MySqlConnection conn = _factory.Create())
            {
                await conn.OpenAsync();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    using (MySqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            Skill s = new Skill();
                            s.Id = reader["id"].ToString();
                            s.Name = reader.GetString("name");
                            return s;
                        }
                    }
                }
            }
            return null;
        }

        // deletes a skill with the given id from the database
        public async Task<bool> DeleteSkillAsync(string id)
        {
            string sql = "DELETE FROM Skill WHERE id = @id;";
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
    }
}
