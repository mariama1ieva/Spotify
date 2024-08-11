using Domain.Entities;

namespace Repository.Repositories.Interfaces
{
    public interface ISongRepository : IBaseRepository<Song>
    {
        Task<bool> AnyAsync(string v);
        Task<List<Song>> GetAllWithDatas();
        Task<Song> GetDataIdWithCategoryArtistAlbum(int id);

    }
}
