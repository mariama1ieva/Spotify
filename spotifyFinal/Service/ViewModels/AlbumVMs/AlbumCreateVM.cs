using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.AlbumVMs
{
    public class AlbumCreateVM
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public IFormFile Photo { get; set; }
        public string? Image { get; set; }
        [Required(ErrorMessage = "Please choose Category")]
        public int? CategoryId { get; set; }
        [Required(ErrorMessage = "Please choose Group")]
        public int? GroupId { get; set; }
        [Required(ErrorMessage = "Please choose Artist")]
        public int? ArtistId { get; set; }
    }
}
