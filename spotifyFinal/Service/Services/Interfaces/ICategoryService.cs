using Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Service.ViewModels.CategoryVMs;

namespace Service.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllAsync();

        Task<bool> AnyAsync(string name);
        Task<CategoryDetailVM> GetByIdAsync(int id);
        Task CreateAsync(CategoryCreateVM model);
        Task UpdateAsync(int id, CategoryEditVM model);
        Task DeleteAsync(int id);
        Task<CategoryWithAlbums> GetCategoryWithAlbums(int id);
        Task<SelectList> GetALlBySelectedAsync();


    }
}
