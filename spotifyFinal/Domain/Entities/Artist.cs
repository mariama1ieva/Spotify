using Domain.Common;

namespace Domain.Entities
{
    public class Artist : BaseEntitiy
    {
        public string FullName { get; set; }
        public List<ArtistSong> ArtistSongs { get; set; }
        public string ImageUrl { get; set; }
        public string AboutImg { get; set; }
        public List<Album> Albums { get; set; }
        public List<ArtistPosition> ArtistPositions { get; set; }
    }
}
