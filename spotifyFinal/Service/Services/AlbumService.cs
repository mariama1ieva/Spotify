using AutoMapper;
using Repository.Repositories.Interfaces;
using Service.Services.Interfaces;
using Service.ViewModels.AlbumVMs;
using Service.ViewModels.CategoryVMs;
using System.Web.Mvc;

namespace Service.Services
{
    public class AlbumService : IAlbumService
    {
        private readonly IAlbumRepository _repository;
        public readonly IMapper _mapper;

        public AlbumService(IAlbumRepository albumRepository, IMapper mapper)
        {
            _repository = albumRepository;
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



        public Task<CategoryDetailVM> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(int id, CategoryEditVM model)
        {
            throw new NotImplementedException();
        }

        public async Task<List<AlbumVM>> GetAllWithCategoryArtistGroup()
        {
            var datas = await _repository.GetAllWithCategoryArtistGroup();

            var model = _mapper.Map<List<AlbumVM>>(datas);

            return model;
        }

        public async Task<SelectList> GetALlBySelectedAsync()
        {
            var datas = await _repository.GetAllWithCategoryArtistGroup();
            return new SelectList(datas, "Id", "Name");
        }
    }
}
