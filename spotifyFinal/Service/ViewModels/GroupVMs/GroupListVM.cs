using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.GroupVMs
{
    public class GroupListVM
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]

        public string? ImageUrl { get; set; }
    }
}
