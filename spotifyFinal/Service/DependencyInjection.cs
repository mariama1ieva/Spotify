using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Repository.Data;
using Service.Helpers;
using Service.Services;
using Service.Services.Interfaces;



namespace Repository
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServiceLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddScoped<ISettingService, SettingService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IAlbumService, AlbumService>();
            services.AddScoped<IArtistService, ArtistService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<ISongService, SongService>();
            services.AddScoped<ISongArtistService, SongArtistService>();
            services.AddScoped<IArtistPositionService, ArtistPositionService>();
            services.AddScoped<IPositionService, PositionService>();
            services.AddScoped<IGroupArtistService, GroupArtistService>();


            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(opt =>
            {
                opt.Password.RequireDigit = true;
                opt.Password.RequiredUniqueChars = 1;
                opt.Password.RequireLowercase = true;
                opt.Password.RequireUppercase = true;
                opt.Password.RequireNonAlphanumeric = true;
                opt.User.RequireUniqueEmail = true;
                opt.SignIn.RequireConfirmedEmail = true;
            });
            return services;
        }
    }
}
