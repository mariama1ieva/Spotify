using Domain.Common;

namespace Domain.Entities
{
    public class Rating : BaseEntitiy
    {
        public double Point { get; set; }
        public int CommentId { get; set; }
        public Comment Comment { get; set; }
    }
}
