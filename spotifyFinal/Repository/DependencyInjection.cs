using Microsoft.Extensions.DependencyInjection;
using Repository.Repositories;
using Repository.Repositories.Interfaces;

namespace Repository
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepositoryLayer(this IServiceCollection services)
        {

            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<ISettingRepository, SettingRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IAlbumRepository, AlbumRepository>();
            services.AddScoped<IArtistRepository, ArtistRepository>();
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<ISongRepository, SongRepository>();
            services.AddScoped<ISongArtistRepository, SongArtistRepository>();
            services.AddScoped<IPositionRepository, PositionRepository>();
            services.AddScoped<IArtistPositionRepository, ArtistPositionRepository>();











            return services;
        }
    }
}
