using Domain.Entities;

namespace Repository.Repositories.Interfaces
{
    public interface IAlbumRepository : IBaseRepository<Album>
    {
        Task<List<Album>> GetAllWithCategoryArtistGroup();
        Task<Album> GetDataIdWithCategoryArtistGroup(int id);

        Task<bool> AnyAsync(string name);

    }
}
