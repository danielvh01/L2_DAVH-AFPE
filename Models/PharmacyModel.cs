using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace L2_DAVH_AFPE.Models
{
    public class PharmacyModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public string Production_Factory { get; set; }

        public double Price { get; set; }

        public int  Quantity { get; set; }
    }
}
