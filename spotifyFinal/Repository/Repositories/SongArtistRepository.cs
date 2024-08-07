using Domain.Entities;
using Repository.Data;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
    public class SongArtistRepository : BaseRepository<Setting>, ISettingRepository
    {
        public SongArtistRepository(AppDbContext context) : base(context)
        {
        }
    }
}
