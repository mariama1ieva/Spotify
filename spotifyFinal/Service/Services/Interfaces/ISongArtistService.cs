using Service.ViewModels.SongArtistVMs;

namespace Service.Services.Interfaces
{
    public interface ISongArtistService
    {
        Task CreateAsync(SongArtistCreateVM model);
        Task UpdateAsync(SongArtistEditVM model);

    }
}
