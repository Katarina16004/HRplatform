using HRplatform.Application.Interfaces;
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
            string sql = "SELECT 1 FROM Skill WHERE LOWER(name) = @name;";
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
    }
}
