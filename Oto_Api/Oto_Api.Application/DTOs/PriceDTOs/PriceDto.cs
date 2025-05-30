﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oto_Api.Application.DTOs.PriceDTOs
{
    public class PriceDto
    {
        public decimal PriceIn { get; set; }


        public decimal PriceSale { get; set; }

        public DateTime EffectiveDate { get; set; } = DateTime.Now;

        public int ProductId { get; set; }
    }
}
