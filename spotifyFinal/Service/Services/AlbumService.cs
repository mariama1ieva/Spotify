using Service.Services.Interfaces;
using Service.ViewModels.Category;

namespace Service.Services
{
    public class AlbumService : IAlbumService
    {
        public Task<bool> AnyAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task CreateAsync(CategoryCreateVM model)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CategoryVM>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<CategoryDetailVM> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(int id, CategoryEditVM model)
        {
            throw new NotImplementedException();
        }
    }
}
