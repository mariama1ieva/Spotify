using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.ArtistVMs
{
    public class ArtistCreateVM
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public IFormFile Photo { get; set; }
        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "The About field is required.")]

        public string AboutImg { get; set; }

        [Required(ErrorMessage = "Please choose Songs!")]

        public IEnumerable<int> SongIds { get; set; }
        [Required(ErrorMessage = "Please choose Positions!")]
        public IEnumerable<int> PositionIds { get; set; }
    }
}
