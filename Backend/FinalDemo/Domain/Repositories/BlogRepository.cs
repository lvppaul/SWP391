﻿using Domain.Models.Entity;
using Microsoft.EntityFrameworkCore;
using SWP391.KCSAH.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public class BlogRepository: GenericRepository<Blog>
    {
        public BlogRepository(KoiCareSystemAtHomeContext context) => _context = context;

        public async Task<List<Blog>> GetAllAsync()
        {
            return await _context.Blogs.Include(p => p.BlogComments).Include(p => p.BlogImages).ToListAsync();
        }

        public async Task<Blog> GetByIdAsync(int id)
        {
            var result = await _context.Blogs.Include(p => p.BlogComments).Include(p => p.BlogImages).FirstOrDefaultAsync(p => p.BlogId.Equals(id));

            return result;
        }
    }
}
