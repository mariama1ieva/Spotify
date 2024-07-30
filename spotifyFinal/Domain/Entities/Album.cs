using Domain.Common;

using System.Text.RegularExpressions;

namespace Domain.Entities
{
    public class Album : BaseEntity
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public List<Song> Songs { get; set; }
        public int? ArtistId { get; set; }
        public Artist Artist { get; set; }
        public int? GroupId { get; set; }
        public Group Group { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
