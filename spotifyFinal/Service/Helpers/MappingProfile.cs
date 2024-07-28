using AutoMapper;
using Domain.Entities;
using Service.ViewModels.Setting;

namespace Service.Helpers
{
    public class MappingProfile : Profile
    {

        public MappingProfile()
        {
            CreateMap<Setting, SettingVM>();
            CreateMap<SettingEditVM, Setting>();
            CreateMap<Setting, SettingListVM>();
            CreateMap<SettingCreateVM, Setting>();

        }
    }
}
