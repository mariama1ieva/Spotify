using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.Category
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
