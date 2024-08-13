using Domain.Entities;

namespace Service.ViewModels.ArtistVMs
{
    public class ArtistDetailVM
    {
        public string? FullName { get; set; }
        public string? ImageUrl { get; set; }
        public string? AboutImg { get; set; }
        public IEnumerable<ArtistPosition> ArtistPositions { get; set; }
        public IEnumerable<ArtistSong> ArtistSongs { get; set; }

    }
}
