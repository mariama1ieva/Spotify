using Domain.Common;

namespace Domain.Entities
{
    public class Group : BaseEntity
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public List<GroupArtist> GroupArtists { get; set; }
        public List<Album> Albums { get; set; }

    }
}
