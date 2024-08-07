using Service.ViewModels.CategoryVMs;
using System.Web.Mvc;

namespace Service.Services.Interfaces
{
    internal interface IGroupService
    {
        Task<IEnumerable<CategoryVM>> GetAllAsync();

        Task<bool> AnyAsync(string name);
        Task<CategoryDetailVM> GetByIdAsync(int id);
        Task CreateAsync(CategoryCreateVM model);
        Task UpdateAsync(int id, CategoryEditVM model);
        Task DeleteAsync(int id);
        Task<CategoryWithAlbums> GetCategoryWithAlbums(int id);
        Task<SelectList> GetALlBySelectedAsync();
    }
}
