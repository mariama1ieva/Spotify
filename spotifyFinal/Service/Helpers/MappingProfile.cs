using AutoMapper;
using Domain.Entities;
using Service.ViewModels.Category;
using Service.ViewModels.Setting;

namespace Service.Helpers
{
    public class MappingProfile : Profile
    {

        public MappingProfile()
        {
            CreateMap<Setting, SettingEditVM>();
            CreateMap<SettingEditVM, Setting>();
            CreateMap<Setting, SettingListVM>();
            CreateMap<SettingCreateVM, Setting>();

            CreateMap<Category, CategoryVM>();
            CreateMap<Category, CategoryDetailVM>();




        }
    }
}
