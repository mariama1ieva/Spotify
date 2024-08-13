using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
    public class ArtistRepository : BaseRepository<Artist>, IArtistRepository
    {
        public ArtistRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<bool> AnyAsync(string fullName)
        {
            return await _context.Artists.AnyAsync(m => m.FullName == fullName);
        }

        public async Task<List<Artist>> GetAllWithDatas()
        {
            return await _entities.Include(c => c.ArtistPositions).ThenInclude(m => m.Position).Include(m => m.ArtistSongs).ThenInclude(m => m.Song).ToListAsync();
        }

        public async Task<Artist> GetAllWithDatasById(int id)
        {
            return await _entities.Include(c => c.ArtistPositions).ThenInclude(m => m.Position).Include(m => m.ArtistSongs).ThenInclude(m => m.Song).FirstOrDefaultAsync(m => m.Id == id);

        }
    }
}
