using Domain.Common;

namespace Domain.Entities
{
    public class Playlist : BaseEntity
    {
        public string Name { get; set; }
        public string? ImgUrl { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public List<MusicPlaylist> MusicPlaylists { get; set; }
    }
}
