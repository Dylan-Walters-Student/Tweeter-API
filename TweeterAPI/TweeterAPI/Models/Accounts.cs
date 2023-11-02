using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace TweeterAPI.Models
{
    public class Accounts
    {
        [NotNull]
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Password { get; set; }

        public string? Email { get; set; }

        public int Likes { get; set; }

        public string? Biography { get; set; }

    }
}
