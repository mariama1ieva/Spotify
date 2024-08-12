using AutoMapper;
using Domain.Entities;
using Repository.Repositories.Interfaces;
using Service.Services.Interfaces;
using Service.ViewModels.PositionVMs;

namespace Service.Services
{
    public class PositionService : IPositionService
    {
        private readonly IPositionRepository _repository;
        public readonly IMapper _mapper;

        public PositionService(IPositionRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<bool> AnyAsync(string name)
        {
            return await _repository.AnyAsync(name.Trim().ToLower());
        }

        public async Task<int> CreateAsync(PositionCreateVM model)
        {
            var positionId = await _repository.CreateAsync(_mapper.Map<Position>(model));
            return positionId;
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(await _repository.GetByIdAsync(id));
        }

        public async Task<PositionDetailVM> GetByIdAsync(int id)
        {
            return _mapper.Map<PositionDetailVM>(await _repository.GetByIdAsync(id));

        }

        public async Task UpdateAsync(int id, PositionEditVM model)
        {
            var dbPosition = await _repository.GetByIdAsync(id);

            var maplbum = _mapper.Map(model, dbPosition);
            maplbum.Id = id;

            await _repository.UpdateAsync(maplbum);
        }

        public async Task<List<PositionListVM>> GetAllWithDatas()
        {
            var datas = await _repository.GetAllWithDatas();

            var model = _mapper.Map<List<PositionListVM>>(datas);

            return model;
        }

        public async Task<PositionDetailVM> GetAllWithDatasById(int id)
        {
            var data = await _repository.GetAllWithDatasById(id);

            return _mapper.Map<PositionDetailVM>(data);
        }
    }
}
