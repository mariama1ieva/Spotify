using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.GroupArtistVMs
{
    public class GroupArtistCreateVM
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public IFormFile Photo { get; set; }
        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "Please choose Group")]
        public int? GroupId { get; set; }
    }
}
