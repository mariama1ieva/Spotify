using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
    public class GroupRepository : BaseRepository<Group>, IGroupRepository
    {
        public GroupRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<bool> AnyAsync(string groupName)
        {
            return await _context.Groups.AnyAsync(m => m.Name == groupName);
        }

    }
}
