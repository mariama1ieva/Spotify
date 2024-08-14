using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.GroupVMs
{
    public class GroupEditVM
    {
        public IFormFile? Photo { get; set; }
        public string? ImageUrl { get; set; }

        [Required]
        public string? Name { get; set; }
    }
}
