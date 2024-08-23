using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Service.ViewModels
{
    public class DashboardVM
    {
        public List<Artist> Artists { get; set; } = null!;
        public List<Album> Albums { get; set; } = null!;
        public List<Song> Songs { get; set; } = null!;
        public List<Category> Categories { get; set; } = null!;
        public List<Position> Positions { get; set; } = null!;
        public List<AppUser> AppUsers { get; set; } = null!;
        public List<IdentityRole> IdentityRoles { get; set; } = null!;

        public int SongCount => Songs.Count;
        public int AlbumCount => Albums.Count;
        public int CategoryCount => Categories.Count;
        public int PositionCount => Positions.Count;
        public int ArtistCount => Artists.Count;


    }
}
