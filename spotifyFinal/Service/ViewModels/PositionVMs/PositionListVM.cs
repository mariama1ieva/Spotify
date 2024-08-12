using Domain.Entities;

namespace Service.ViewModels.PositionVMs
{
    public class PositionListVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<ArtistPosition> ArtistPositions { get; set; }
    }
}
