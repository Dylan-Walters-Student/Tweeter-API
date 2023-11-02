using System.Diagnostics.CodeAnalysis;

namespace TweeterAPI.Models
{
    public class Accounts
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Password { get; set; }

        public string? Email { get; set; }

        public int Likes { get; set; }

        public string? Biography { get; set; }

    }
}
