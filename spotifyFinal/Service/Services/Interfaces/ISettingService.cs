using Service.ViewModels.Setting;

namespace Service.Services.Interfaces
{
    public interface ISettingService
    {
        Task<IEnumerable<SettingListVM>> GetAllAsync();
        Task<bool> AnyAsync(string key);
        Task<Dictionary<string, string>> GetAll();
        Task<SettingVM> GetByIdAsync(int id);
        Task CreateAsync(SettingCreateVM model);
        //Task<SettingVM> GetEqualId(int id);
        Task UpdateAsync(SettingEditVM model);
        Task DeleteAsync(int id);
    }
}
