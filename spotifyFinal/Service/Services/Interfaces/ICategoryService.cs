using Service.ViewModels.Category;
using Service.ViewModels.Setting;

namespace Service.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryVM>> GetAllAsync();

        Task<bool> AnyAsync(string key);
        Task<CategoryDetailVM> GetByIdAsync(int id);
        Task CreateAsync(SettingCreateVM model);
        Task UpdateAsync(int id, SettingEditVM model);
        Task DeleteAsync(int id);
    }
}
