using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.ArtistVMs
{
    public class ArtistCreateVM
    {
        [Required, MaxLength(50)]
        public string FullName { get; set; }
        public IFormFile Photo { get; set; } = null!;
        public IFormFile AboutImg { get; set; } = null!;
        [ValidateNever]
        public ICollection<Song> Songs { get; set; } = null!;

        [ValidateNever]
        public ICollection<Position> Positions { get; set; } = null!;

        [Required(ErrorMessage = "Please choose Songs!")]

        public IEnumerable<int> SongIds { get; set; }
        [Required(ErrorMessage = "Please choose Positions!")]
        public IEnumerable<int> PositionIds { get; set; }
    }
}
