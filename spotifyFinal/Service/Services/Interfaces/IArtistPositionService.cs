using Service.ViewModels.ArtistPositionVMs;

namespace Service.Services.Interfaces
{
    public interface IArtistPositionService
    {
        Task CreateAsync(ArtistPositionCreateVM model);
        Task UpdateAsync(ArtistPositionEditVM model);
    }
}
