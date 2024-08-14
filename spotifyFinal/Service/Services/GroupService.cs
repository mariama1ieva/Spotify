using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repository.Repositories.Interfaces;
using Service.Services.Interfaces;
using Service.ViewModels.GroupVMs;


namespace Service.Services
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _repository;
        public readonly IMapper _mapper;

        public GroupService(IGroupRepository groupRepository, IMapper mapper)
        {
            _repository = groupRepository;
            _mapper = mapper;
        }
        public async Task<bool> AnyAsync(string groupName)
        {
            return await _repository.AnyAsync(groupName.Trim().ToLower());

        }

        public async Task CreateAsync(GroupCreateVM model)

        {
            await _repository.CreateAsync(_mapper.Map<Group>(model));

        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(await _repository.GetByIdAsync(id));

        }

        public async Task<IEnumerable<GroupListVM>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<GroupListVM>>(await _repository.GetAllAsync());

        }

        public async Task<SelectList> GetALlBySelectedAsync()
        {
            var datas = await _repository.GetAllAsync();
            return new SelectList(datas, "Id", "Name");
        }

        public async Task<GroupDetailVM> GetByIdAsync(int id)
        {
            return _mapper.Map<GroupDetailVM>(await _repository.GetByIdAsync(id));

        }


        public async Task UpdateAsync(int id, GroupEditVM model)
        {
            var dbGroup = await _repository.GetByIdAsync(id);

            var mapGroup = _mapper.Map(model, dbGroup);

            await _repository.UpdateAsync(mapGroup);
        }


    }

}
