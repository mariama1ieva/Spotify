using AutoMapper;
using Domain.Entities;
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

        public async Task<int> CreateAsync(SongCreateVM model)
        {
            var song = _repository.CreateAsync(_mapper.Map<Song>(model));
            return song.Id;
        }
    }
}
