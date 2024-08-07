using Domain.Entities;

namespace Repository.Repositories.Interfaces
{
    public interface IAlbumRepository : IBaseRepository<Album>
    {
        Task<List<Album>> GetAllWithCategoryArtistGroup();
    }
}
