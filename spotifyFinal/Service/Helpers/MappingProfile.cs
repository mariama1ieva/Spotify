﻿using AutoMapper;
using Domain.Entities;
using Service.ViewModels.AlbumVMs;
using Service.ViewModels.ArtistPositionVMs;
using Service.ViewModels.ArtistVMs;
using Service.ViewModels.CategoryVMs;
using Service.ViewModels.GroupArtistVMs;
using Service.ViewModels.GroupVMs;
using Service.ViewModels.PositionVMs;
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


            CreateMap<GroupArtist, GroupArtistListVM>();
            CreateMap<GroupArtistCreateVM, GroupArtist>();
            CreateMap<GroupArtist, GroupArtistDetailVM>();
            CreateMap<GroupArtistEditVM, GroupArtist>();


            CreateMap<SongCreateVM, Song>();
            CreateMap<Song, SongListVM>();

            CreateMap<Song, SongDetailVM>();
            CreateMap<SongArtistCreateVM, ArtistSong>();
            CreateMap<SongArtistEditVM, Song>();
            CreateMap<ArtistSong, SongArtistListVM>();
            CreateMap<SongEditVM, Song>();
            CreateMap<SongDetailVM, SongEditVM>();


            CreateMap<PositionCreateVM, Position>();
            CreateMap<Position, PositionListVM>();
            CreateMap<Position, PositionDetailVM>();
            CreateMap<ArtistPositionCreateVM, ArtistPosition>();
            CreateMap<ArtistPositionEditVM, ArtistPosition>();
            CreateMap<PositionEditVM, Position>();


            CreateMap<Group, GroupListVM>();
            CreateMap<Group, GroupDetailVM>();
            CreateMap<GroupCreateVM, Group>();
            CreateMap<GroupEditVM, Group>();



            CreateMap<Artist, ArtistSelectVM>();
            CreateMap<ArtistCreateVM, Artist>();
            CreateMap<Artist, ArtistListVM>();
            CreateMap<Artist, ArtistDetailVM>();
            CreateMap<ArtistPositionCreateVM, ArtistPosition>();
            CreateMap<ArtistPositionEditVM, ArtistPosition>();
            CreateMap<ArtistEditVM, Artist>();



        }

    }
}
