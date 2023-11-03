using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;
using TweeterAPI.Models;

namespace TweeterAPI.Controllers
{
    [Route("api/Comments")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly string _connectionString;

        public CommentsController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("NotTwitter_DBConnection");
        }

        [HttpPost(Name = "CreateComments")]
        public async Task<IActionResult> Create([FromBody] Comments comment)
        {
            string commandText = "INSERT INTO Comments (source_id, source_type, comment_message, comment_likes) " +
                                "VALUES (@source_id, @source_type, @comment_message, @comment_likes);";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(commandText, conn))
                {
                    conn.Open();

                    try
                    {
                        cmd.Parameters.Add("@source_id", SqlDbType.UniqueIdentifier);
                        cmd.Parameters["@source_id"].Value = comment.SourceId;

                        cmd.Parameters.Add("@source_type", SqlDbType.NVarChar);
                        cmd.Parameters["@source_type"].Value = comment.SourceType;

                        cmd.Parameters.Add("@comment_message", SqlDbType.NVarChar);
                        cmd.Parameters["@comment_message"].Value = comment.Message;

                        cmd.Parameters.Add("@comment_likes", SqlDbType.Int);
                        cmd.Parameters["@comment_likes"].Value = comment.Likes;

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

        [HttpGet(Name = "GetComments")]
        public IEnumerable<Comments> Get()
        {
            var _comments = new List<Comments>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Comments", conn))
                {
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        _comments.Add(new Comments
                        {
                            Id = (int)reader["comment_id"],
                            SourceId = (Guid)reader["source_id"],
                            Message = reader["comment_message"].ToString(),
                            Likes = (int)reader["comment_likes"],
                        });
                    }
                }
            }

            return _comments;
        }

        [HttpPut(Name = "UpdateComments")]
        public async Task<IActionResult> Update([FromBody] Comments comments)
        {
            string commandText = $"UPDATE Comments SET comment_message = @comment_message WHERE comment_id = @comment_id";
                
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(commandText, conn))
                {
                    conn.Open();

                    try
                    {
                        cmd.Parameters.Add("@comment_id", SqlDbType.Int);
                        cmd.Parameters["@comment_id"].Value = comments.Id;

                        cmd.Parameters.Add("@comment_message", SqlDbType.NVarChar);
                        cmd.Parameters["@comment_message"].Value = comments.Message;

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

        [HttpDelete(Name = "DeleteComments")]
        public async Task<IActionResult> Delete([FromBody] Comments comments)
        {
            string commandText = "DELETE FROM Comments WHERE comment_id LIKE @comment_id";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(commandText, conn))
                {
                    conn.Open();

                    cmd.Parameters.Add("@comment_id", SqlDbType.Int);
                    cmd.Parameters["@comment_id"].Value = comments.Id;

                    cmd.ExecuteNonQuery();
                    return Ok("success");
                }
            }
        }
    }
}
