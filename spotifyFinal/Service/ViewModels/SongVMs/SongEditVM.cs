using Microsoft.AspNetCore.Http;
using Service.ViewModels.SongArtistVMs;
using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.SongVMs
{
    public class SongEditVM
    {
        public IFormFile? PhotoUrl { get; set; }
        public string? ImageUrl { get; set; }
        [Required]
        public string Name { get; set; }
        public IFormFile? SongUrl { get; set; }
        public string? Path { get; set; }
        public string Color { get; set; }
        public double? PointRayting { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public int? AlbumId { get; set; }
        public string? AlbumName { get; set; }
        public IEnumerable<int> ArtistIds { get; set; }
        public IEnumerable<SongArtistListVM>? Artists { get; set; }
    }
}
