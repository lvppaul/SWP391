﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Dto.Request
{
    public class VipPackageRequestDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public int Price { get; set; }
    }
}
