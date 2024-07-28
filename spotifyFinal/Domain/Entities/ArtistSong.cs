using Domain.Common;

namespace Domain.Entities
{
    public class ArtistSong : BaseEntitiy
    {
        public int ArtistId { get; set; }
        public Artist Artist { get; set; }
        public int SongId { get; set; }
        public Song Song { get; set; }

    }
}
