using AutoMapper;
using Repository.Repositories.Interfaces;
using Service.Services.Interfaces;
using Service.ViewModels.Category;
using Service.ViewModels.Setting;

namespace Service.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;
        public readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _repository = categoryRepository;
            _mapper = mapper;
        }
        public Task<bool> AnyAsync(string key)
        {
            throw new NotImplementedException();
        }

        public Task CreateAsync(SettingCreateVM model)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CategoryVM>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<CategoryVM>>(await _repository.GetAllAsync());


        }

        public async Task<CategoryDetailVM> GetByIdAsync(int id)
        {
            return _mapper.Map<CategoryDetailVM>(await _repository.GetByIdAsync(id));

        }

        public Task UpdateAsync(int id, SettingEditVM model)
        {
            throw new NotImplementedException();
        }


    }
}
