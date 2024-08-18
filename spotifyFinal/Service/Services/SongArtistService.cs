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

        public async Task<IEnumerable<int>> GetAllArtistIdsBySongId(int songId)
        {
            var songArtists = await _repository.GetAllAsync();

            var assignedArtistIds = songArtists
                .Where(sa => sa.SongId == songId)
                .Select(sa => sa.ArtistId)
                .ToList();

            return assignedArtistIds;
        }

        public async Task<IEnumerable<SongArtistListVM>> GetAllBySongIdAsync(int songId)
        {
            IEnumerable<ArtistSong> songArtists = await _repository.GetAllAsync();

            return _mapper.Map<IEnumerable<SongArtistListVM>>(songArtists.Where(m => m.SongId == songId));
        }
    }
}
