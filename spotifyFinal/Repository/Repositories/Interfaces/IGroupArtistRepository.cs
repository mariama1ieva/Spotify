using Domain.Entities;

namespace Repository.Repositories.Interfaces
{
    public interface IGroupArtistRepository : IBaseRepository<GroupArtist>
    {
        Task<List<GroupArtist>> GetAllWithGroup();
        Task<GroupArtist> GetAllWithGroup(int id);

        Task<bool> AnyAsync(string fullName);
    }
}
