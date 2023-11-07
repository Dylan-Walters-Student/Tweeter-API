using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace TweeterAPI.Models
{
    public class Posts
    {
        public Guid Id { get; set; }

        public string Message { get; set; }

        public int Likes { get; set; }

        public DateTime UploadTime { get; set; }
    }
}
