using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace L2_DAVH_AFPE.Models
{
    public class Cart
    {
        public string clientName { get; set; }
        public string NIT { get; set; }
        public string address { get; set; }
        public double amount { get; set; }

        public Data.DoubleLinkedList<Data.Drug> products = new Data.DoubleLinkedList<Data.Drug>();
    }
}
