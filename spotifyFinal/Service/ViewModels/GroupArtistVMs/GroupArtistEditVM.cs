using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.GroupArtistVMs
{
    public class GroupArtistEditVM
    {
        [Required]
        public string FullName { get; set; }

        public IFormFile? Photo { get; set; }

        public string? ImageUrl { get; set; }

        public string? GroupName { get; set; }

        public int GroupId { get; set; }
    }
}
