using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.SongVMs
{
    public class SongEditVM
    {
        [ValidateNever]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }

        public IFormFile? Photo { get; set; }

        public IFormFile? Audio { get; set; }
        [ValidateNever]
        public string? Path { get; set; }
        [ValidateNever]
        public string? ImageUrl { get; set; }
        public string? Color { get; set; }
        public DateTime CreateDate { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        [ValidateNever]
        public ICollection<ArtistSong> ArtistSongs { get; set; } = new List<ArtistSong>();
        public int? AlbumId { get; set; }
        public Album Album { get; set; }

        public List<int> ArtistsIds { get; set; } = new List<int>();
    }
}
