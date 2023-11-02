﻿using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Security.Claims;
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
            _connectionString = configuration.GetConnectionString("NewPerformanceTest1_DBConnection");
        }

        [HttpPost(Name = "CreateAccounts")]
        public async Task<IActionResult> Create([FromBody] Accounts accounts)
        {
            string commandText = "INSERT INTO Accounts (account_name, account_password, account_email, account_likes, account_biography) " +
                                "VALUES (@account_name, @account_password, @account_email, @account_likes, @account_biography);";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(commandText, conn))
                {
                    conn.Open();

                    try
                    {
                        cmd.Parameters.Add("@account_name", SqlDbType.NVarChar);
                        cmd.Parameters["@account_name"].Value = accounts.Name;

                        cmd.Parameters.Add("@account_password", SqlDbType.NVarChar);
                        cmd.Parameters["@account_password"].Value = accounts.Password;

                        cmd.Parameters.Add("@account_email", SqlDbType.NVarChar);
                        cmd.Parameters["@account_email"].Value = accounts.Email;

                        cmd.Parameters.Add("@account_likes", SqlDbType.Int);
                        cmd.Parameters["@account_likes"].Value = accounts.Likes;

                        cmd.Parameters.Add("@account_biography", SqlDbType.NVarChar);
                        cmd.Parameters["@account_biography"].Value = accounts.Biography;

                        cmd.ExecuteNonQuery();
                        return Ok("success");
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }
                }
            }
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
                            Id = (Guid)reader["account_id"],
                            Name = reader["account_name"].ToString(),
                            Password = reader["account_password"].ToString(),
                            Email = reader["account_email"].ToString(),
                            Likes = (int)reader["account_likes"],
                            Biography = reader["account_biography"].ToString() 
                        });
                    }
                }
            }

            return _accounts;
        }

        [HttpPut(Name = "UpdateAccounts")]
        public async Task<IActionResult> Update([FromBody] Accounts accounts, UpdateType type)
        {
            string commandText;

            switch (type)
            {
                case UpdateType.AccountName:
                    commandText = $"UPDATE Accounts SET account_name = @account_name WHERE account_id = @account_id";
                    break;
                case UpdateType.AccountPassword:
                    commandText = $"UPDATE Accounts SET account_password = @account_password WHERE account_id = @account_id";
                    break;
                case UpdateType.AccountEmail:
                    commandText = $"UPDATE Accounts SET account_email = @account_email WHERE account_id = @account_id";
                    break;
                case UpdateType.accountBiography:
                    commandText = $"UPDATE Accounts SET account_biography = @account_biography WHERE account_id = @account_id";
                    break;
                default:
                    return BadRequest("Please return a proper type.");
            }
                
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(commandText, conn))
                {
                    conn.Open();

                    try
                    {
                        cmd.Parameters.Add("@account_id", SqlDbType.UniqueIdentifier);
                        cmd.Parameters["@account_id"].Value = accounts.Id;

                        switch (type)
                        {
                            case UpdateType.AccountName:
                                cmd.Parameters.Add("@account_name", SqlDbType.NVarChar);
                                cmd.Parameters["@account_name"].Value = accounts.Name;
                                break;
                            case UpdateType.AccountPassword:
                                cmd.Parameters.Add("@account_password", SqlDbType.NVarChar);
                                cmd.Parameters["@account_password"].Value = accounts.Password;
                                break;
                            case UpdateType.AccountEmail:
                                cmd.Parameters.Add("@account_email", SqlDbType.NVarChar);
                                cmd.Parameters["@account_email"].Value = accounts.Email;
                                break;
                            case UpdateType.accountBiography:
                                cmd.Parameters.Add("@account_biography", SqlDbType.NVarChar);
                                cmd.Parameters["@account_biography"].Value = accounts.Biography;
                                break;
                        }

                        cmd.ExecuteNonQuery();

                        return Ok("success");
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }
                }
            }
        }

        [HttpDelete(Name = "DeleteAccounts")]
        public async Task<IActionResult> Delete([FromBody] Accounts accounts)
        {
            string commandText = "DELETE FROM Accounts WHERE account_id LIKE @account_id";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(commandText, conn))
                {
                    conn.Open();

                    cmd.Parameters.Add("@account_id", SqlDbType.UniqueIdentifier);
                    cmd.Parameters["@account_id"].Value = accounts.Id;

                    cmd.ExecuteNonQuery();
                    return Ok("success");
                }
            }
        }
    }
}
