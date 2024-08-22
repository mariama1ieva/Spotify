using Domain.Entities;

namespace Repository.Repositories.Interfaces
{
    public interface IAccountRepository
    {
        Task<List<AppUser>> GetAll();
        Task<IList<string>> GetRoles(AppUser user);


    }
}
