using Domain.Entities;

namespace Service.ViewModels
{
    public class SidebarVM
    {
        public int SongCount { get; set; }
        public List<Playlist> Playlists { get; set; }

    }
}
