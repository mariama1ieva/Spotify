using AutoMapper;
using Domain.Entities;
using Repository.Repositories.Interfaces;
using Service.Services.Interfaces;
using Service.ViewModels.ArtistPositionVMs;

namespace Service.Services
{
    public class ArtistPositionService : IArtistPositionService
    {
        private readonly IArtistPositionRepository _repository;
        public readonly IMapper _mapper;

        public ArtistPositionService(IArtistPositionRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task CreateAsync(ArtistPositionCreateVM model)
        {
            await _repository.CreateAsync(_mapper.Map<ArtistPosition>(model));
        }

        public async Task UpdateAsync(ArtistPositionEditVM model)
        {
            await _repository.UpdateAsync(_mapper.Map<ArtistPosition>(model));

        }
    }
}
