using Service.ViewModels.SongVMs;
using System.Web.Mvc;

namespace Service.Services.Interfaces
{
    public interface ISongService
    {
        Task<bool> AnyAsync(string name);
        Task<int> CreateAsync(SongCreateVM model);
        //Task<CategoryDetailVM> GetByIdAsync(int id);
        //Task UpdateAsync(int id, CategoryEditVM model);
        Task DeleteAsync(int id);
        Task<List<SongListVM>> GetAllWithDatas();
        Task<SelectList> GetALlBySelectedAsync();
    }
}
