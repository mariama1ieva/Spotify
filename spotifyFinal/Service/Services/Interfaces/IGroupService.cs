using Microsoft.AspNetCore.Mvc.Rendering;
using Service.ViewModels.GroupVMs;

namespace Service.Services.Interfaces
{
    public interface IGroupService
    {
        Task<IEnumerable<GroupListVM>> GetAllAsync();

        Task<bool> AnyAsync(string groupName);
        Task<GroupDetailVM> GetByIdAsync(int id);

        Task CreateAsync(GroupCreateVM model);
        Task UpdateAsync(int id, GroupEditVM model);
        Task DeleteAsync(int id);
        Task<SelectList> GetALlBySelectedAsync();
    }
}
