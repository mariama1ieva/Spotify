using Microsoft.AspNetCore.Mvc.Rendering;
using Service.ViewModels.PositionVMs;

namespace Service.Services.Interfaces
{
    public interface IPositionService
    {
        Task<bool> AnyAsync(string name);
        Task<int> CreateAsync(PositionCreateVM model);
        Task<PositionDetailVM> GetByIdAsync(int id);
        Task UpdateAsync(int id, PositionEditVM model);
        Task DeleteAsync(int id);
        Task<List<PositionListVM>> GetAllWithDatas();

        Task<PositionDetailVM> GetAllWithDatasById(int id);
        Task<SelectList> GetALlBySelectedAsync();


    }
}
