﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
    public class SettingRepository : BaseRepository<Setting>, ISettingRepository
    {
        public SettingRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Dictionary<string, string>> GetAll()
        {
            return await _context.Settings.ToDictionaryAsync(m => m.Key, m => m.Value);
        }
    }
}