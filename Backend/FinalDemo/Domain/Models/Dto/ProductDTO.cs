﻿using KCSAH.APIServer.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Dto
{
    public class ProductDTO
    {
        public string ProductId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }

        public bool? Status { get; set; }

        public CategoryDTO category { get; set; }

        public string UserId { get; set; }
    }
}
