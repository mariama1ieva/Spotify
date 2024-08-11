using Domain.Entities;

namespace Service.ViewModels.SongVMs
{
    public class SongDetailVM
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string ImageUrl { get; set; }
        public string Color { get; set; }
        public double? PointRayting { get; set; }
        public string CategoryName { get; set; }
        public string AlbumName { get; set; }
        public IEnumerable<ArtistSong> ArtistSongs { get; set; }

    }
}
