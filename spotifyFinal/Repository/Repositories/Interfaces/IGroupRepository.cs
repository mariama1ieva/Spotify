using Domain.Entities;

namespace Repository.Repositories.Interfaces
{
    public interface IGroupRepository : IBaseRepository<Group>
    {
        Task<bool> AnyAsync(string groupName);

    }
}
