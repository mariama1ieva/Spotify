using Domain.Entities;
using Repository.Data;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
    public class ArtistPositionRepository : BaseRepository<ArtistPosition>, IArtistPositionRepository
    {
        public ArtistPositionRepository(AppDbContext context) : base(context)
        {
        }
    }
}
