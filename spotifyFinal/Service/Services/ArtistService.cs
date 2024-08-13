using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repository.Repositories.Interfaces;
using Service.Services.Interfaces;
using Service.ViewModels.ArtistVMs;

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
        public async Task<bool> AnyAsync(string fullName)
        {
            return await _repository.AnyAsync(fullName.Trim().ToLower());

        }



        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(await _repository.GetByIdAsync(id));

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

        public async Task<ArtistDetailVM> GetByIdAsync(int id)
        {
            return _mapper.Map<ArtistDetailVM>(await _repository.GetByIdAsync(id));

        }

        public async Task UpdateAsync(int id, ArtistEditVM model)

        {
            var dbArtist = await _repository.GetAllWithDatasById(id);

            var maplbum = _mapper.Map(model, dbArtist);
            maplbum.Id = id;

            await _repository.UpdateAsync(maplbum);
        }

        public async Task<List<ArtistListVM>> GetAllWithDatas()
        {
            var datas = await _repository.GetAllWithDatas();

            var model = _mapper.Map<List<ArtistListVM>>(datas);

            return model;
        }
        public async Task<ArtistDetailVM> GetAllWithDatasById(int id)
        {
            var data = await _repository.GetAllWithDatasById(id);

            return _mapper.Map<ArtistDetailVM>(data);
        }


        public async Task<int> CreateAsync(ArtistCreateVM model)
        {
            var artistId = await _repository.CreateAsync(_mapper.Map<Artist>(model));
            return artistId;
        }
    }
}
