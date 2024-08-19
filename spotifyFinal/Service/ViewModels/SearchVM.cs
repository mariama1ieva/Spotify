using Domain.Entities;

namespace Service.ViewModels
{
    public class SearchVM
    {
        public ICollection<Song> Songs { get; set; }
        public ICollection<Playlist> Playlists { get; set; }
        public ICollection<ArtistSong> ArtistSongs { get; set; }
        public ICollection<Artist> Artists { get; set; }
        public ICollection<Album> Albums { get; set; }
        public ICollection<Category> Categories { get; set; }
        public ICollection<MusicPlaylist> MusicPlaylists { get; set; }
    }
}
