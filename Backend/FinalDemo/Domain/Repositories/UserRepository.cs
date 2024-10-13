﻿using Domain.Models;
using Microsoft.EntityFrameworkCore;
using SWP391.KCSAH.Repository.Base;
using SWP391.KCSAH.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391.KCSAH.Repository.KCSAH.Repository
{
    public class UserRepository: GenericRepository<ApplicationUser>
    {
        public UserRepository(KoiCareSystemAtHomeContext context) => _context = context;
    }
}
