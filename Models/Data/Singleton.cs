using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataStructures;

namespace L2_DAVH_AFPE.Models.Data
{

    public sealed class Singleton
    {
        public double total;
        public string sd;
        public DoubleLinkedList<string> options = new DoubleLinkedList<string>();
        private readonly static Singleton _instance = new Singleton();
        public DoubleLinkedList<PharmacyModel> orders;
        public DoubleLinkedList<PharmacyModel> inventory;
        public BinaryTree<Drug> guide;
        public bool fileUpload = false;
        public string tree = "";
        public int contOrder = 0;
        private Singleton()
        {
            orders = new DoubleLinkedList<PharmacyModel>();
            inventory = new DoubleLinkedList<PharmacyModel>();
            guide = new BinaryTree<Drug>();
        }

        public static Singleton Instance
        {
            get
            {
                return _instance;
            }
        }
        public string getPrice(string product)
        {
            return "$" + Instance.inventory.Get(Instance.guide.Find(x => x.name.CompareTo(product), Singleton.Instance.guide.Root).numberline).Price;
        }

        public double totalre()
        {
            total = 0;
            for (int i = 0; i < orders.Length; i++)
            {
                var x = orders.Get(i);
                total += x.Price * x.Quantity;
            }
            return total;
        }

        public void Resuply()
        {
            for(int i = 0; i < Models.Data.Singleton.Instance.inventory.Length; i++)
            {
                PharmacyModel item = Models.Data.Singleton.Instance.inventory.Get(i);
                if (item.Quantity == 0)
                {
                    Random r = new Random();
                    item.Quantity = r.Next(1, 15);
                    Singleton.Instance.guide.Insert(new Drug { name = item.Name, numberline = i }, Singleton.Instance.guide.Root);
                }
            }
        }

        public string PrintTree(TreeNode<Drug> node)
        {
            if (node == null)
            {
                return "";
            }
            if (node.right != null)
            {
                PrintTree(node.right);
            }
            tree += node.value.name + " Numberline: " + node.value.numberline + "\r\n";
            
            if (node.left != null)
            {
                PrintTree(node.left);
            }
            return tree;
        }

    }
}