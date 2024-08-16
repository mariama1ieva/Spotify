using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Repository.Repositories.Interfaces;
using Service.Helpers;
using Service.Services.Interfaces;
using Service.ViewModels.GroupArtistVMs;


namespace Service.Services
{
    public class GroupArtistService : IGroupArtistService
    {
        private readonly IGroupArtistRepository _repository;
        public readonly IMapper _mapper;

        public GroupArtistService(IGroupArtistRepository _groupArtistRepository, IMapper mapper)
        {
            _repository = _groupArtistRepository;
            _mapper = mapper;
        }

        public async Task<bool> AnyAsync(string fullName)
        {
            return await _repository.AnyAsync(fullName.Trim().ToLower());
        }

        public async Task CreateAsync(GroupArtistCreateVM model)
        {
            await _repository.CreateAsync(_mapper.Map<GroupArtist>(model));
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(await _repository.GetByIdAsync(id));
        }

        public async Task<IEnumerable<GroupArtistListVM>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<GroupArtistListVM>>(await _repository.GetAllAsync());
        }

        public async Task<List<GroupArtistListVM>> GetAllWithGroup()
        {
            var datas = await _repository.GetAllWithGroup();

            var model = _mapper.Map<List<GroupArtistListVM>>(datas);

            return model;
        }

        public async Task<GroupArtistDetailVM> GetAllWithGroup(int id)
        {
            var data = await _repository.GetAllWithGroup(id);

            return _mapper.Map<GroupArtistDetailVM>(data);
        }

        public async Task<GroupArtistDetailVM> GetByIdAsync(int id)
        {
            var a = await _repository.GetByIdAsync(id);

            return _mapper.Map<GroupArtistDetailVM>(a);
        }


        public async Task UpdateAsync(int id, GroupArtistEditVM model)
        {
            var data = await _repository.GetByIdAsync(id);

            var map = _mapper.Map(model, data);
            map.Id = id;

            await _repository.UpdateAsync(map);
        }
        public void ConfigureServices(IServiceCollection services)
        {
            // Other service configurations

            // Register AutoMapper with the DI container
            services.AddAutoMapper(typeof(MappingProfile)); // or typeof(YourStartupClass)
        }
    }
}
