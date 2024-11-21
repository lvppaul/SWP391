﻿using Microsoft.EntityFrameworkCore;
using SWP391.KCSAH.Repository.Base;
using SWP391.KCSAH.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.KCSAH.Repository.KCSAH.Repository
{
    public class CategoryRepository : GenericRepository<Category>
    {
        public CategoryRepository(KoiCareSystemAtHomeContext context) => _context = context;

        public async Task<List<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetByIdAsync(string id)
        {
            var result = await _context.Categories.FirstOrDefaultAsync(p => p.CategoryId.Equals(id));

            return result;
        }
    }
}