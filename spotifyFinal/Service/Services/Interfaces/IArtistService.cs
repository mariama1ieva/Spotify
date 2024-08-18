using Microsoft.AspNetCore.Mvc.Rendering;
using Service.ViewModels.ArtistVMs;

namespace Service.Services.Interfaces
{
    public interface IArtistService
    {
        Task<IEnumerable<ArtistSelectVM>> GetAllAsync();
        Task<bool> AnyAsync(string fullName);
        Task<ArtistDetailVM> GetByIdAsync(int id);
        Task<int> CreateAsync(ArtistCreateVM model);
        Task UpdateAsync(int id, ArtistEditVM model);
        Task DeleteAsync(int id);
        Task<SelectList> GetALlBySelectedAsync();
        Task<SelectList> GetAllSelectListAsync(IEnumerable<int> artistIds);
        Task<List<ArtistListVM>> GetAllWithDatas();
        Task<ArtistDetailVM> GetAllWithDatasById(int id);
    }
}
