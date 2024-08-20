using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public string? Fullname { get; set; }

        public bool IsActive { get; set; }
        public List<Playlist> Playlists { get; set; }
        public List<Comment> Comments { get; set; }
        public decimal? Wallet { get; set; }
        public string? ImageUrl { get; set; }
    }
}
