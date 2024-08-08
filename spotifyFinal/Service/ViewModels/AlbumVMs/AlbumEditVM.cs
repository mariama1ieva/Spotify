using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.AlbumVMs
{
    public class AlbumEditVM
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public IFormFile Photo { get; set; }
        public string? Image { get; set; }
        [Required]

        public string Category { get; set; }
        [Required]

        public int? CategoryId { get; set; }
        [Required]

        public string Group { get; set; }
        [Required]

        public int? GroupId { get; set; }
        [Required]

        public string Artist { get; set; }
        [Required]

        public int? ArtistId { get; set; }
    }
}
