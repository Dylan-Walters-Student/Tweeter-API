using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using TweeterAPI.Models;

namespace TweeterAPI.Controllers
{
    [Route("api/Posts")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly string _connectionString;

        public PostsController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("NotTwitter_DBConnection");
        }

        [HttpPost(Name = "CreatePosts")]
        public async Task<IActionResult> Create([FromBody] Posts posts)
        {
            string commandText = "INSERT INTO Posts (posts_id, posts_message, posts_likes) " +
                                "VALUES (@posts_id, @posts_message, @posts_likes);";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(commandText, conn))
                {
                    conn.Open();

                    try
                    {
                        cmd.Parameters.Add("@posts_id", SqlDbType.UniqueIdentifier);
                        cmd.Parameters["@posts_id"].Value = posts.Id;

                        cmd.Parameters.Add("@posts_message", SqlDbType.NVarChar);
                        cmd.Parameters["@posts_message"].Value = posts.Message;

                        cmd.Parameters.Add("@posts_likes", SqlDbType.Int);
                        cmd.Parameters["@posts_likes"].Value = posts.Likes;

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

        [HttpGet(Name = "GetPosts")]
        public IEnumerable<Posts> Get()
        {
            var _posts = new List<Posts>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Posts", conn))
                {
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        _posts.Add(new Posts
                        {
                            Id = (Guid)reader["posts_id"],
                            Message = reader["posts_message"].ToString(),
                            Likes = (int)reader["posts_likes"],
                        });
                    }
                }
            }

            return _posts;
        }

        [HttpPut(Name = "UpdatePosts")]
        public async Task<IActionResult> Update([FromBody] Comments comments, UpdateType type)
        {
            string commandText = $"UPDATE Comments SET comment_message = @comment_message WHERE comment_id = @comment_id";
                
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(commandText, conn))
                {
                    conn.Open();

                    try
                    {
                        cmd.Parameters.Add("@comment_id", SqlDbType.UniqueIdentifier);
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

        [HttpDelete(Name = "DeletePosts")]
        public async Task<IActionResult> Delete([FromBody] Comments comments)
        {
            string commandText = "DELETE FROM Comments WHERE comment_id LIKE @comment_id";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(commandText, conn))
                {
                    conn.Open();

                    cmd.Parameters.Add("@comment_id", SqlDbType.UniqueIdentifier);
                    cmd.Parameters["@comment_id"].Value = comments.Id;

                    cmd.ExecuteNonQuery();
                    return Ok("success");
                }
            }
        }
    }
}
