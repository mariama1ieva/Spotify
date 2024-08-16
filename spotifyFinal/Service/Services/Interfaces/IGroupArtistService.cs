using Service.ViewModels.GroupArtistVMs;



namespace Service.Services.Interfaces
{
    public interface IGroupArtistService
    {
        Task<bool> AnyAsync(string fullName);
        Task<GroupArtistDetailVM> GetByIdAsync(int id);
        Task CreateAsync(GroupArtistCreateVM model);
        Task UpdateAsync(int id, GroupArtistEditVM model);
        Task DeleteAsync(int id);
        Task<List<GroupArtistListVM>> GetAllWithGroup();
        Task<IEnumerable<GroupArtistListVM>> GetAllAsync();
        Task<GroupArtistDetailVM> GetAllWithGroup(int id);
    }
}
