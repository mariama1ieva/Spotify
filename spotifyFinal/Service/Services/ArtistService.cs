using AutoMapper;
using Repository.Repositories.Interfaces;
using Service.Services.Interfaces;
using Service.ViewModels.CategoryVMs;
using System.Web.Mvc;

namespace Service.Services
{
    public class ArtistService : IArtistService
    {
        private readonly IArtistRepository _repository;
        public readonly IMapper _mapper;

        public ArtistService(IArtistRepository artistRepository, IMapper mapper)
        {
            _repository = artistRepository;
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

        public Task UpdateAsync(int id, CategoryEditVM model)
        {
            throw new NotImplementedException();
        }
    }
}
