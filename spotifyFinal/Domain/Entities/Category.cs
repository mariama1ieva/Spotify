using Domain.Common;

namespace Domain.Entities
{
    public class Category : BaseEntitiy
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public string ImageUrl { get; set; }
        public List<Song> Songs { get; set; }
        public List<Album> Albums { get; set; }


    }
}
