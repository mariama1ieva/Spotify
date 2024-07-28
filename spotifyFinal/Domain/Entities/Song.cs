using Domain.Common;

namespace Domain.Entities
{
    public class Song : BaseEntitiy
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string ImageUrl { get; set; }
        public string Color { get; set; }
        public double? PointRayting { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int? AlbumId { get; set; }
        public Album Album { get; set; }
        public ICollection<ArtistSong> ArtistSongs { get; set; }
        public ICollection<MusicPlaylist> MusicPlaylists { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<WishlistItem> WishlistItems { get; set; }
    }
}
