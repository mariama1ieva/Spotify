using AutoMapper;
using Domain.Entities;
using Service.ViewModels.AlbumVMs;
using Service.ViewModels.ArtistVMs;
using Service.ViewModels.CategoryVMs;
using Service.ViewModels.Setting;
using Service.ViewModels.SongArtistVMs;
using Service.ViewModels.SongVMs;

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
            CreateMap<CategoryCreateVM, Category>();
            CreateMap<CategoryEditVM, Category>();
            CreateMap<Category, CategoryWithAlbums>();

            CreateMap<Album, AlbumVM>();
            CreateMap<AlbumCreateVM, Album>();
            CreateMap<Album, AlbumDetailVM>();
            CreateMap<AlbumEditVM, Album>();




            CreateMap<SongCreateVM, Song>();
            CreateMap<Song, SongListVM>();
            CreateMap<Song, SongDetailVM>();
            CreateMap<SongArtistCreateVM, ArtistSong>();
            CreateMap<SongArtistEditVM, Song>();
            CreateMap<SongEditVM, Song>();






            CreateMap<Artist, ArtistSelectVM>();







        }
    }
}
