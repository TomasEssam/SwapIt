﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.BL.DTOs
{
    public class ServiceFilterDto
    {
        public string ServiceName { get; set; }
        public float ServicePrice { get; set; }
        public int ServiceProviderId { get; set; }
        public int CategoryId { get; set; }

    }
}