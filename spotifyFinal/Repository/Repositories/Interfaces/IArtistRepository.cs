using Domain.Entities;

namespace Repository.Repositories.Interfaces
{
    public interface IArtistRepository : IBaseRepository<Artist>
    {
        Task<bool> AnyAsync(string fullName);
        Task<List<Artist>> GetAllWithDatas();
        Task<Artist> GetAllWithDatasById(int id);
    }
}
