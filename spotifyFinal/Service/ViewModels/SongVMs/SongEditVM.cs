using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.SongVMs
{
    public class SongEditVM
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public IFormFile SongUrl { get; set; }
        [Required]
        public string Path { get; set; }
        [Required]
        public IFormFile PhotoUrl { get; set; }
        public string ImageUrl { get; set; }
        [Required]
        public string Color { get; set; }
        [Required]
        public double? PointRayting { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public int? AlbumId { get; set; }
        [Required]
        public IEnumerable<int> ArtistIds { get; set; }
    }
}
