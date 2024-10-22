﻿using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.SongVMs
{
    public class SongCreateVM
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public IFormFile SongUrl { get; set; }
        public string? Path { get; set; }
        [Required]
        public IFormFile PhotoUrl { get; set; }
        public string? ImageUrl { get; set; }
        [Required]
        public string Color { get; set; }
        [Required]
        [Range(0, 5, ErrorMessage = "Rating must be between 0 and 5.")]
        public double? PointRayting { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public int? AlbumId { get; set; }
        [Required]
        public IEnumerable<int> ArtistIds { get; set; }
    }
}
