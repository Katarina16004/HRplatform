using HRplatform.Application.Interfaces;
using HRplatform.Domain;
using HRplatform.Infrastructure.Db;
using MySqlConnector;

namespace HRplatform.Infrastructure.Repositories
{
    public class SkillRepository : ISkillRepository
    {
        private readonly MySqlConnectionFactory _factory;

        public SkillRepository(MySqlConnectionFactory factory)
        {
            _factory = factory;
        }
        public async Task<string> CreateSkillAsync(Domain.Skill skill)
        {
            string sql = "INSERT INTO Skill (id, name) VALUES (@id, @name);";
            using (MySqlConnection conn = _factory.Create())
            {
                await conn.OpenAsync();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", skill.Id);
                    cmd.Parameters.AddWithValue("@name", skill.Name);

                    await cmd.ExecuteNonQueryAsync();
                    return skill.Id;
                }
            }
        }

        public async Task<bool> SkillExistsByNameAsync(string name)
        {
            string sql = "SELECT 1 FROM Skill WHERE name = @name;";
            using (MySqlConnection conn = _factory.Create())
            {
                await conn.OpenAsync();
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@name", name);

                    object? result = await cmd.ExecuteScalarAsync();
                    return result != null;
                }
            }
        }

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

        public async Task<Skill?> GetSkillByNameAsync(string name)
        {
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
