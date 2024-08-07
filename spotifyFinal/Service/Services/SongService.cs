using AutoMapper;
using Domain.Entities;
using Repository.Repositories.Interfaces;
using Service.Services.Interfaces;
using Service.ViewModels.SongVMs;
using System.Web.Mvc;

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

        public async Task<int> CreateAsync(SongCreateVM model)
        {
            var song = _repository.CreateAsync(_mapper.Map<Song>(model));
            return song.Id;
        }

        public async Task<bool> AnyAsync(string name)
        {
            return await _repository.AnyAsync(name.Trim().ToLower());
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<SongListVM>> GetAllWithDatas()
        {
            var datas = await _repository.GetAllWithDatas();

            var model = _mapper.Map<List<SongListVM>>(datas);

            return model;
        }

        public Task<SelectList> GetALlBySelectedAsync()
        {
            throw new NotImplementedException();
        }
    }
}
