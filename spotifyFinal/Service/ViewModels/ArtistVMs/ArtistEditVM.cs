using Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.ArtistVMs
{
    public class ArtistEditVM
    {
        [Required(ErrorMessage = "Can not be empty!")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Can not be empty!")]
        public string AboutImg { get; set; }


        public IFormFile? Photo { get; set; }
        public string? ImageUrl { get; set; }

        public IEnumerable<int> PositionIds { get; set; }
        public IEnumerable<ArtistPosition> Position { get; set; }
        public IEnumerable<int> SongIds { get; set; }
        public IEnumerable<ArtistSong> Song { get; set; }
    }
}
