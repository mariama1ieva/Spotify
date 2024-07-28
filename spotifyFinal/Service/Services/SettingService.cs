using AutoMapper;
using Domain.Entities;
using Repository.Repositories.Interfaces;
using Service.Services.Interfaces;
using Service.ViewModels.Setting;

namespace Service.Services
{
    public class SettingService : ISettingService
    {
        private readonly ISettingRepository _repository;
        public readonly IMapper _mapper;


        public SettingService(ISettingRepository settingRepository, IMapper mapper)
        {
            _repository = settingRepository;
            _mapper = mapper;
        }

        public async Task<Dictionary<string, string>> GetAll()
        {
            return await _repository.GetAll();

        }

        public async Task<IEnumerable<SettingVM>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<SettingVM>>(await _repository.GetAllAsync());
        }
        public async Task<SettingVM> GetByIdAsync(int id)
        {
            return _mapper.Map<SettingVM>(await _repository.GetByIdAsync(id));
        }

        public async Task CreateAsync(SettingVM setting)
        {
            await _repository.CreateAsync(_mapper.Map<Setting>(setting));
        }

        public async Task UpdateAsync(SettingVM model)
        {
            await _repository.UpdateAsync(_mapper.Map<Setting>(model));
        }
    }
}
