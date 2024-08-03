using Service.ViewModels.Category;

namespace Service.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryVM>> GetAllAsync();

        Task<bool> AnyAsync(string name);
        Task<CategoryDetailVM> GetByIdAsync(int id);
        Task CreateAsync(CategoryCreateVM model);
        Task UpdateAsync(int id, CategoryEditVM model);
        Task DeleteAsync(int id);
        Task<CategoryWithAlbums> GetCategoryWithAlbums(int id);

    }
}
