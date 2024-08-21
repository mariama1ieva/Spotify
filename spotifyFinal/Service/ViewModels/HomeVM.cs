

using Domain.Entities;

namespace Service.ViewModels
{
    public class HomeVM
    {
        public ICollection<Album> Albums { get; set; }
        public ICollection<Song> Songs { get; set; }
        public ICollection<Artist> Artists { get; set; }
        public ICollection<Playlist> Playlists { get; set; }
        public List<Album> CategoryAlbums { get; set; }
        public string Category { get; set; }
        public Dictionary<string, List<Album>> AlbumsByCategory { get; set; }


    }
}
