using Service.ViewModels.AlbumVMs;
using Service.ViewModels.CategoryVMs;
using System.Web.Mvc;

namespace Service.Services.Interfaces
{
    public interface IAlbumService
    {
        Task<bool> AnyAsync(string name);
        Task<CategoryDetailVM> GetByIdAsync(int id);
        Task CreateAsync(CategoryCreateVM model);
        Task UpdateAsync(int id, CategoryEditVM model);
        Task DeleteAsync(int id);
        Task<List<AlbumVM>> GetAllWithCategoryArtistGroup();
        Task<SelectList> GetALlBySelectedAsync();

    }
}
