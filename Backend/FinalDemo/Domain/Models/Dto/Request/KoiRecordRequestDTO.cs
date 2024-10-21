﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Models.Dto.Request
{
    public class KoiRecordRequestDTO
    {
        public int KoiId { get; set; }
        public string UserId { get; set; } = null!;

        public int Weight { get; set; }

        public int Length { get; set; }
        public DateTime UpdatedTime { get; set; }
    }
}