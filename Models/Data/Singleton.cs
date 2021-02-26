using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace L2_DAVH_AFPE.Models.Data
{

    public sealed class Singleton
    {
        private readonly static Singleton _instance = new Singleton();
        public DoubleLinkedList<Cart> HandcraftedList;
        private Singleton()
        {
            HandcraftedList = new DoubleLinkedList<Cart>();
        }

        public static Singleton Instance
        {
            get
            {
                return _instance;
            }
        }

    }
}