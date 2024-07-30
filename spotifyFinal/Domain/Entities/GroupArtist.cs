using Domain.Common;

namespace Domain.Entities
{
    public class GroupArtist : BaseEntity
    {
        public string FullName { get; set; }
        public string ImageUrl { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
    }
}
