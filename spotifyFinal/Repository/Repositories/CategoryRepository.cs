using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Repositories.Interfaces;
using System.Web.Mvc;

namespace Repository.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<bool> AnyAsync(string name)
        {
            return await _context.Categories.AnyAsync(m => m.Name == name);

        }

        public Task<SelectList> GetALlBySelectedAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Category> GetAllWithAlbums(int id)
        {
            return await _entities.Include(e => e.Albums).FirstOrDefaultAsync(m => m.Id == id);
        }

    }
}
