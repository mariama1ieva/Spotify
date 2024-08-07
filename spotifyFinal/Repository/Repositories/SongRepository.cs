using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
    public class SongRepository : BaseRepository<Song>, ISongRepository
    {
        public SongRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<bool> AnyAsync(string name)
        {
            return await _context.Songs.AnyAsync(m => m.Name == name);
        }

        public async Task<List<Song>> GetAllWithDatas()
        {
            return await _entities.Include(e => e.Album).Include(m => m.Category).Include(c => c.ArtistSongs).ThenInclude(m => m.Artist).ToListAsync();
        }
    }
}
