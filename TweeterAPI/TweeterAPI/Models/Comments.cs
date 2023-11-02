using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace TweeterAPI.Models
{
    public class Comments
    {
        public int Id { get; set; }

        public Guid SourceId { get; set; }

        public string Message { get; set; }

        public int Likes { get; set; }
    }
}
