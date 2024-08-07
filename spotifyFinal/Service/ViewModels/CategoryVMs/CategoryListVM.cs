using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.CategoryVMs
{
    public class CategoryListVM
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Color { get; set; }

        [Required]
        public string ImageUrl { get; set; }


    }
}
