using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Service.ViewModels
{
    public class ProfileVM
    {
        public string UserName { get; set; }
        public decimal? WalletAmount { get; set; }
        public List<Playlist> Playlists { get; set; }
        public string ProfilePhotoUrl { get; set; }
        public IFormFile ProfileImg { get; set; }
    }
}
