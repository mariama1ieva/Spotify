using Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.SongVMs
{
    public class SongEditVM
    {
        [Required]
        public string Name { get; set; }
        public IFormFile? SongUrl { get; set; }
        public string? Path { get; set; }
        public IFormFile? PhotoUrl { get; set; }
        public string? ImageUrl { get; set; }
        [Required]
        public string Color { get; set; }
        public double? PointRayting { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }

        public int? AlbumId { get; set; }
        public string? AlbumName { get; set; }

        public IEnumerable<int> ArtistIds { get; set; }
        public IEnumerable<ArtistSong> ArtistFullName { get; set; }

        //public IEnumerable<int> PlaylistIds { get; set; }

        //public IEnumerable<Playlist> PlaylistName { get; set; }


    }
}
