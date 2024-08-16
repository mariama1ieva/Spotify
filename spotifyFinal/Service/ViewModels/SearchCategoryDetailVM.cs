using Domain.Entities;

namespace Service.ViewModels
{
    public class SearchCategoryDetailVM
    {
        public ICollection<Song> Songs { get; set; }
        public ICollection<Artist> Artists { get; set; }
        public Artist Artist { get; set; }
        public ICollection<Album> Albums { get; set; }
        public ICollection<Category> Categories { get; set; }
        public Category Category { get; set; }
    }
}
