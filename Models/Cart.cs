using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataStructures;
using L2_DAVH_AFPE.Models.Data;

namespace L2_DAVH_AFPE.Models
{
    public class Cart
    {
        public int ID { get; set; }
        public string clientName { get; set; }
        public string NIT { get; set; }
        public string address { get; set; }
        public double amount { get; set; }

        public DoubleLinkedList<PharmacyModel> products;

        public Cart()
        {
            ID = ++Singleton.Instance.contCarts;
            clientName = "";
            NIT = "";
            address = "";
            Singleton.Instance.totalre();
            amount = Singleton.Instance.total;
            products = new DoubleLinkedList<PharmacyModel>();
            foreach (var product in Singleton.Instance.orders)
            {
                products.InsertAtEnd(product);
            }
        }
    }
}
