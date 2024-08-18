using Service.ViewModels.SongArtistVMs;

namespace Service.Services.Interfaces
{
    public interface ISongArtistService
    {
        Task CreateAsync(SongArtistCreateVM model);
        Task UpdateAsync(SongArtistEditVM model);
        Task<IEnumerable<int>> GetAllArtistIdsBySongId(int songId);
        Task<IEnumerable<SongArtistListVM>> GetAllBySongIdAsync(int songId);
    }
}
