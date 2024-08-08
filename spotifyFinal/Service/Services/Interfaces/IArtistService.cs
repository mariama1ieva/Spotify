using Microsoft.AspNetCore.Mvc.Rendering;
using Service.ViewModels.ArtistVMs;
using Service.ViewModels.CategoryVMs;

namespace Service.Services.Interfaces
{
    public interface IArtistService
    {
        Task<IEnumerable<ArtistSelectVM>> GetAllAsync();
        Task<bool> AnyAsync(string name);
        Task<CategoryDetailVM> GetByIdAsync(int id);
        Task CreateAsync(CategoryCreateVM model);
        Task UpdateAsync(int id, CategoryEditVM model);
        Task DeleteAsync(int id);
        Task<SelectList> GetALlBySelectedAsync();
    }
}
