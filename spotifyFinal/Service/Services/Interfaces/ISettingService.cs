using Service.ViewModels.Setting;

namespace Service.Services.Interfaces
{
    public interface ISettingService
    {
        Task<IEnumerable<SettingVM>> GetAllAsync();
        Task<Dictionary<string, string>> GetAll();
        Task<SettingVM> GetByIdAsync(int id);
        Task CreateAsync(SettingVM setting);
        //Task<SettingVM> GetEqualId(int id);
        Task UpdateAsync(SettingVM model);
    }
}
