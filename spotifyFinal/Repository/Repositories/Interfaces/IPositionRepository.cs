using Domain.Entities;

namespace Repository.Repositories.Interfaces
{
    public interface IPositionRepository : IBaseRepository<Position>
    {
        Task<bool> AnyAsync(string name);
        Task<List<Position>> GetAllWithDatas();
        Task<Position> GetAllWithDatasById(int id);



    }
}
