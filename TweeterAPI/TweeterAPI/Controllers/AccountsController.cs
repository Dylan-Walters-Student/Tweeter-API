using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using TweeterAPI.Models;

namespace TweeterAPI.Controllers
{
    [Route("api/Accounts")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly string _connectionString;

        public AccountsController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("NewPerformanceTest1");
        }

        [HttpGet(Name = "GetAccounts")]
        public IEnumerable<Accounts> Get()
        {
            var _accounts = new List<Accounts>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Accounts", conn))
                {
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        _accounts.Add(new Accounts
                        {
                            Id = (int)reader["class_id"],
                            Name = reader["account_name"].ToString(),
                            Password = reader["account_password"].ToString(),
                            Email = reader["account_email"].ToString(),
                            Likes = (int)reader["account_likes"],
                            Biography = reader["account_biogrphy"].ToString() 
                        });
                    }
                }
            }

            return _accounts;
        }
    }
}
