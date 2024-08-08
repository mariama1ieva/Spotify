using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repository.Repositories.Interfaces;
using Service.Services.Interfaces;
using Service.ViewModels.ArtistVMs;
using Service.ViewModels.CategoryVMs;


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

        public async Task<IEnumerable<ArtistSelectVM>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<ArtistSelectVM>>(await _repository.GetAllAsync());
        }

        public async Task<SelectList> GetALlBySelectedAsync()
        {
            var datas = await GetAllAsync();
            return new SelectList(datas, "Id", "FullName");
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
