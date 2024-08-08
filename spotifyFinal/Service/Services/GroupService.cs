using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repository.Repositories.Interfaces;
using Service.Services.Interfaces;
using Service.ViewModels.CategoryVMs;

namespace Service.Services
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _repository;
        public readonly IMapper _mapper;

        public GroupService(IGroupRepository groupRepository, IMapper mapper)
        {
            _repository = groupRepository;
            _mapper = mapper;
        }
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

        public async Task<SelectList> GetALlBySelectedAsync()
        {
            var datas = await _repository.GetAllAsync();
            return new SelectList(datas, "Id", "Name");
        }

        public Task<CategoryDetailVM> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<CategoryWithAlbums> GetCategoryWithAlbums(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(int id, CategoryEditVM model)
        {
            throw new NotImplementedException();
        }
    }
}
