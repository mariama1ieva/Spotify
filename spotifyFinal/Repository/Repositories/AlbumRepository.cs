using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
    public class AlbumRepository : BaseRepository<Album>, IAlbumRepository
    {
        public AlbumRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<bool> AnyAsync(string name, string image)
        {
            return await _context.Albums.AnyAsync(m => m.Name == name && m.Image == image);
        }

        public async Task<List<Album>> GetAllWithCategoryArtistGroup()
        {
            return await _entities.Include(e => e.Artist).Include(m => m.Category).Include(c => c.Group).ToListAsync();
        }
    }
}
