using Domain.Entities;
using Repository.Data;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
    public class AlbumRepository : BaseRepository<Album>, IAlbumRepository
    {
        public AlbumRepository(AppDbContext context) : base(context)
        {
        }

    }
}
