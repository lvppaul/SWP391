﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Models.Dto.Request
{
    public class OrderRequestDTO
    {
        public string UserId { get; set; } = null!;

        public int ShopId { get; set; }

        public string FullName { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Street { get; set; } = null!;

        public string District { get; set; } = null!;

        public string City { get; set; } = null!;

        public string Country { get; set; } = null!;

        public int PaymentMethodId { get; set; }

        //public double TotalPrice { get; set; }
        [JsonIgnore]
        public DateTime CreateDate { get; set; } = DateTime.Now;

        public string OrderStatus { get; set; } = null!;
        public List<OrderDetailRequestDTO> orderDetails { get; set; }
        [JsonIgnore]
        public double TotalPrice { get; set; }
    }
}
