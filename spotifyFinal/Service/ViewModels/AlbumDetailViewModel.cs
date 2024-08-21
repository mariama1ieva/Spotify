using Domain.Entities;

namespace Service.ViewModels
{
    public class AlbumDetailViewModel
    {
        public Album Album { get; set; }
        public int TotalSongCount { get; set; }
        public List<Album> OtherAlbums { get; set; }
    }
}
