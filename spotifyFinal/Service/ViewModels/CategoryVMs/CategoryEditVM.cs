using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.CategoryVMs
{
    public class CategoryEditVM
    {
        public IFormFile? Photo { get; set; }
        public string? ImageUrl { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Color { get; set; }
    }
}
