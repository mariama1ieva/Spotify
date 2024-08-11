using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repository.Repositories.Interfaces;
using Service.Services.Interfaces;
using Service.ViewModels.AlbumVMs;

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

        public async Task<bool> AnyAsync(string name)
        {
            return await _repository.AnyAsync(name.Trim().ToLower());
        }

        public async Task CreateAsync(AlbumCreateVM model)
        {
            await _repository.CreateAsync(_mapper.Map<Album>(model));
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(await _repository.GetByIdAsync(id));
        }

        public async Task<AlbumDetailVM> GetByIdAsync(int id)
        {
            var a = await _repository.GetByIdAsync(id);

            return _mapper.Map<AlbumDetailVM>(a);
        }

        public async Task UpdateAsync(int id, AlbumEditVM model)
        {
            var dbAlbum = await _repository.GetByIdAsync(id);

            var maplbum = _mapper.Map(model, dbAlbum);
            maplbum.Id = id;

            await _repository.UpdateAsync(maplbum);
        }

        public async Task<List<AlbumVM>> GetAllWithCategoryArtistGroup()
        {
            var datas = await _repository.GetAllWithCategoryArtistGroup();

            var model = _mapper.Map<List<AlbumVM>>(datas);

            return model;
        }

        public async Task<SelectList> GetALlBySelectedAsync()
        {
            var datas = await GetAllAsync();
            return new SelectList(datas, "Id", "Name");
        }

        public async Task<IEnumerable<AlbumVM>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<AlbumVM>>(await _repository.GetAllAsync());
        }

        public async Task<AlbumDetailVM> GetDataIdWithCategoryArtistGroup(int id)
        {
            var data = await _repository.GetDataIdWithCategoryArtistGroup(id);

            return _mapper.Map<AlbumDetailVM>(data);
        }
    }

}
