using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repository.Repositories.Interfaces;
using Service.Services.Interfaces;
using Service.ViewModels.SongVMs;


namespace Service.Services
{
    public class SongService : ISongService
    {
        private readonly ISongRepository _repository;
        public readonly IMapper _mapper;

        public SongService(ISongRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<SongDetailVM> GetByIdAsync(int id)
        {
            return _mapper.Map<SongDetailVM>(await _repository.GetByIdAsync(id));
        }
        public async Task<int> CreateAsync(SongCreateVM model)
        {
            var songId = await _repository.CreateAsync(_mapper.Map<Song>(model));
            return songId;
        }

        public async Task<bool> AnyAsync(string name)
        {
            return await _repository.AnyAsync(name.Trim().ToLower());
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(await _repository.GetByIdAsync(id));
        }

        public async Task<List<SongListVM>> GetAllWithDatas()
        {
            var datas = await _repository.GetAllWithDatas();

            var model = _mapper.Map<List<SongListVM>>(datas);

            return model;
        }

        public async Task<SelectList> GetALlBySelectedAsync()
        {
            var datas = await GetAllWithDatas();
            return new SelectList(datas, "Id", "Name");
        }

        public async Task UpdateAsync(int id, SongEditVM model)
        {

            var dbAlbum = await _repository.GetByIdAsync(id);

            var maplbum = _mapper.Map(model, dbAlbum);
            maplbum.Id = id;

            await _repository.UpdateAsync(maplbum);
        }

        public async Task<SongDetailVM> GetDataIdWithCategoryArtistAlbum(int id)
        {
            var data = await _repository.GetDataIdWithCategoryArtistAlbum(id);

            return _mapper.Map<SongDetailVM>(data);
        }
    }
}
