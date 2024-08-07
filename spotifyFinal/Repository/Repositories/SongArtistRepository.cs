using Domain.Entities;
using Repository.Data;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
    public class SongArtistRepository : BaseRepository<ArtistSong>, ISongArtistRepository

    {
        public SongArtistRepository(AppDbContext context) : base(context)
        {
        }
    }
}
