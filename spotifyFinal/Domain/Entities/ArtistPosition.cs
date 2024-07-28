using Domain.Common;

namespace Domain.Entities
{
    public class ArtistPosition : BaseEntitiy
    {
        public int ArtistId { get; set; }
        public Artist Artist { get; set; }
        public int PositionId { get; set; }
        public Position Position { get; set; }
    }
}
