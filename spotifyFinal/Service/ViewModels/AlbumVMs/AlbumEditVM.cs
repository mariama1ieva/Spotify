using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.AlbumVMs
{
    public class AlbumEditVM
    {
        [Required]
        public string Name { get; set; }

        public IFormFile? Photo { get; set; }

        public string? Image { get; set; }

        public string? CategoryName { get; set; }

        public int CategoryId { get; set; }

        public string? GroupName { get; set; }

        public int GroupId { get; set; }

        public string? ArtistFullName { get; set; }

        public int ArtistId { get; set; }
    }
}
