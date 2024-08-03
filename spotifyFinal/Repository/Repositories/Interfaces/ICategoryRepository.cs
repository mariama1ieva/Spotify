using Domain.Entities;

namespace Repository.Repositories.Interfaces
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Task<bool> AnyAsync(string name);
        Task<Category> GetAllWithAlbums(int id);


    }
}
