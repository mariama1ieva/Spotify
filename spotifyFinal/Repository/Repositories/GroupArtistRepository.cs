using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
    public class GroupArtistRepository : BaseRepository<GroupArtist>, IGroupArtistRepository
    {
        public GroupArtistRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<bool> AnyAsync(string fullName)
        {
            return await _entities.AnyAsync(m => m.FullName == fullName);

        }

        public async Task<List<GroupArtist>> GetAllWithGroup()
        {
            return await _entities.Include(c => c.Group).ToListAsync();

        }

        public async Task<GroupArtist> GetAllWithGroup(int id)
        {
            return await _entities.Include(c => c.Group).FirstOrDefaultAsync(m => m.Id == id);

        }
    }
}
