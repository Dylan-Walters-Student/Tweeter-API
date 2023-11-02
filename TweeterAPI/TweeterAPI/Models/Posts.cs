using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace TweeterAPI.Models
{
    public class Posts
    {
        [NotNull]
        public Guid Id { get; set; }

        [NotNull]
        public Guid SourceId { get; set; }

        public string Message { get; set; }

        public int Likes { get; set; }
    }
}
