using AutoMapper;
using Domain.Entities;
using Repository.Repositories.Interfaces;
using Service.Services.Interfaces;
using Service.ViewModels.SongArtistVMs;

namespace Service.Services
{
    public class SongArtistService : ISongArtistService
    {
        private readonly ISongArtistRepository _repository;
        public readonly IMapper _mapper;

        public SongArtistService(ISongArtistRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task CreateAsync(SongArtistCreateVM model)
        {
            await _repository.CreateAsync(_mapper.Map<ArtistSong>(model));
        }

        public async Task UpdateAsync(SongArtistEditVM model)
        {
            await _repository.UpdateAsync(_mapper.Map<ArtistSong>(model));

        }
    }
}
