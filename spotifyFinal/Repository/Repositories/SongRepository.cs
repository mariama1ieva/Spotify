using Domain.Entities;
using Repository.Data;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
    public class SongRepository : BaseRepository<Song>, ISongRepository
    {
        public SongRepository(AppDbContext context) : base(context)
        {
        }
    }
}
