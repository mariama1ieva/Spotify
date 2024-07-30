namespace Domain.Common
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public bool SoftDelete { get; set; } = false;
    }
}
