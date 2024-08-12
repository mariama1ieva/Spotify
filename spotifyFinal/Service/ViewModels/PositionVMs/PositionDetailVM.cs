using Domain.Entities;

namespace Service.ViewModels.PositionVMs
{
    public class PositionDetailVM
    {
        public string Name { get; set; }
        public IEnumerable<ArtistPosition> ArtistPositions { get; set; }

    }
}
