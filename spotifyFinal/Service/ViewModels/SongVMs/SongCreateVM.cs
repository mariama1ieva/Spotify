using Microsoft.AspNetCore.Http;

namespace Service.ViewModels.SongVMs
{
    public class SongCreateVM
    {
        public string Name { get; set; }
        public IFormFile SongUrl { get; set; }
        public string Path { get; set; }
        public IFormFile PhotoUrl { get; set; }
        public string ImageUrl { get; set; }
        public string Color { get; set; }
        public double? PointRayting { get; set; }
        public int CategoryId { get; set; }
        public int? AlbumId { get; set; }
        public IEnumerable<int> ArtistIds { get; set; }
    }
}
