using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.CategoryVMs
{
    public class CategoryUpdateVM
    {
        public int? Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Color { get; set; }

        [Required]
        public string? Image { get; set; }
        public IFormFile? Photo { get; set; }
    }
}
