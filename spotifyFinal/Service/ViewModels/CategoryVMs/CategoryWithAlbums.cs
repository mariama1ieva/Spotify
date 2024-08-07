using Domain.Entities;

namespace Service.ViewModels.CategoryVMs
{
    public class CategoryWithAlbums
    {
        public string? Name { get; set; }
        public IEnumerable<Album>? Albums { get; set; }
    }
}
