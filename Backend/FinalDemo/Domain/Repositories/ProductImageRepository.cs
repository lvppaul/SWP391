﻿using Domain.Models.Dto.Response;
using Domain.Models.Entity;
using Microsoft.EntityFrameworkCore;
using SWP391.KCSAH.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public class ProductImageRepository : GenericRepository<ProductImage>
    {
        public ProductImageRepository(KoiCareSystemAtHomeContext context) => _context = context;

        public async Task<List<ProductImage>> GetImageByProductId(int id)
        {
            return await _context.ProductImages.Where(p => p.ProductId.Equals(id)).ToListAsync();
        }
    }
}
