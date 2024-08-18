
using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Repository.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)

        {
        }

        public DbSet<Setting> Settings { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<ArtistSong> ArtistSongs { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<ArtistPosition> ArtistPositions { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupArtist> GroupArtists { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Playlist> Playlist { get; set; }
        public DbSet<MusicPlaylist> MusicPlaylists { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<WishlistItem> WishlistItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .ConfigureWarnings(warnings => warnings.Ignore(CoreEventId.NavigationBaseIncludeIgnored));
        }


    }
}
