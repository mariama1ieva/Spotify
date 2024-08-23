using Domain.Entities;

namespace Service.ViewModels
{
    public class PlaylistsViewModel
    {
        public IEnumerable<Playlist> MusicPlaylists { get; set; }
        public string CurrentUserId { get; set; }
    }
}
