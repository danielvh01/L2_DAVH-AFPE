using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace L2_DAVH_AFPE.Models.Data
{

    public sealed class Singleton
    {
        public DoubleLinkedList<string> options = new DoubleLinkedList<string>();
        private readonly static Singleton _instance = new Singleton();
        public DoubleLinkedList<Cart> orders;
        public DoubleLinkedList<PharmacyModel> inventory;
        public Tree<Drug> guide;
        public string tree = "";
        private Singleton()
        {
            orders = new DoubleLinkedList<Cart>();
            inventory = new DoubleLinkedList<PharmacyModel>();
            guide = new Tree<Drug>();
        }

        public static Singleton Instance
        {
            get
            {
                return _instance;
            }
        }

        public void Traverse(TreeNode<Drug> node)
        {
            if (node == null)
            {
                return;
            }
            if (node.left != null)
            {
                Traverse(node.left);
            }
            options.InsertAtEnd(node.value.name);
            if (node.right != null)
            {
                Traverse(node.right);
            }
        }

        public void PrintTree(TreeNode<Drug> node)
        {
            if (node == null)
            {
                return;
            }
            if (node.left != null)
            {
                Traverse(node.left);
            }
            tree += node.value.name + "\n";
            
            if (node.right != null)
            {
                Traverse(node.right);
            }
        }

    }
}