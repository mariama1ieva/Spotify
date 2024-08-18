using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<bool> AnyAsync(string name)
        {
            var a = await _entities.AnyAsync(m => m.Name.Trim().ToLower() == name);
            return a;
        }

        public async Task<Category> GetAllWithAlbums(int id)
        {
            return await _entities.Include(e => e.Albums).FirstOrDefaultAsync(m => m.Id == id);
        }

    }
}
