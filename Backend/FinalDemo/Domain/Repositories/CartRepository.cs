﻿using Domain.Models.Entity;
using Microsoft.EntityFrameworkCore;
using SWP391.KCSAH.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.KCSAH.Repository.KCSAH.Repository
{
    public class CartRepository : GenericRepository<Cart>
    {
        public CartRepository(KoiCareSystemAtHomeContext context) => _context = context;

        public async Task<List<Cart>> GetAllAsync()
        {
            return await _context.Carts.ToListAsync();
        }

        public async Task<Cart> GetByIdAsync(string id)
        {
            var result = await _context.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(p => p.UserId.Equals(id));

            return result;
        }
        public async Task<Cart> GetByCartIdAsync(int id)
        {
            var result = await _context.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(p => p.CartId == id);

            return result;
        }
    }
}
