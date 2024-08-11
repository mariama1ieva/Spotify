using Microsoft.AspNetCore.Mvc.Rendering;
using Service.ViewModels.SongVMs;


namespace Service.Services.Interfaces
{
    public interface ISongService
    {
        Task<bool> AnyAsync(string name);
        Task<int> CreateAsync(SongCreateVM model);
        Task<SongDetailVM> GetByIdAsync(int id);
        Task<int> UpdateAsync(int id, SongEditVM model);
        Task DeleteAsync(int id);
        Task<List<SongListVM>> GetAllWithDatas();
        Task<SelectList> GetALlBySelectedAsync();
        Task<SongDetailVM> GetDataIdWithCategoryArtistAlbum(int id);

    }
}
