using Microsoft.AspNetCore.Mvc.Rendering;
using Service.ViewModels.AlbumVMs;

namespace Service.Services.Interfaces
{
    public interface IAlbumService
    {
        Task<bool> AnyAsync(string name);
        Task<AlbumDetailVM> GetByIdAsync(int id);
        Task CreateAsync(AlbumCreateVM model);
        Task UpdateAsync(int id, AlbumEditVM model);
        Task DeleteAsync(int id);
        Task<List<AlbumVM>> GetAllWithCategoryArtistGroup();
        Task<SelectList> GetALlBySelectedAsync();
        Task<IEnumerable<AlbumVM>> GetAllAsync();
        Task<AlbumDetailVM> GetDataIdWithCategoryArtistGroup(int id);



    }
}
