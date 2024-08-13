using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
    public class PositionRepository : BaseRepository<Position>, IPositionRepository
    {
        public PositionRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<bool> AnyAsync(string name)
        {
            return await _context.Positions.AnyAsync(m => m.Name == name);
        }


        public async Task<List<Position>> GetAllWithDatas()
        {
            return await _entities.Include(c => c.ArtistPositions).ThenInclude(m => m.Artist).ToListAsync();
        }

        public async Task<Position> GetAllWithDatasById(int id)
        {
            return await _entities.Include(c => c.ArtistPositions).ThenInclude(m => m.Artist).FirstOrDefaultAsync(m => m.Id == id);

        }
    }
}
