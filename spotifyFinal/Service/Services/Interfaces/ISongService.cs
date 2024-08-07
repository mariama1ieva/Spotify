using Service.ViewModels.SongVMs;

namespace Service.Services.Interfaces
{
    public interface ISongService
    {
        Task<int> CreateAsync(SongCreateVM model);
    }
}
