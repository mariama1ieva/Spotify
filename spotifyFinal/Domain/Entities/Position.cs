using Domain.Common;

namespace Domain.Entities
{
    public class Position : BaseEntitiy
    {
        public string Name { get; set; }
        public List<ArtistPosition> ArtistPositions { get; set; }
    }
}
