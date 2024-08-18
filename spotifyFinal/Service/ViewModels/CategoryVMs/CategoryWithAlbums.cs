using Domain.Entities;

namespace Service.ViewModels.CategoryVMs
{
    public class CategoryWithAlbums
    {
        public string? Color { get; set; }
        public string? Name { get; set; }
        public IEnumerable<Album>? Albums { get; set; }
        public ICollection<Artist> Artists { get; set; }
    }
}
