using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.GroupVMs
{
    public class GroupCreateVM
    {

        [Required]
        public string? Name { get; set; }
        [Required]
        public IFormFile Photo { get; set; }
        public string? ImageUrl { get; set; }
    }
}
