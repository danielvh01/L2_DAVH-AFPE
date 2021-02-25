using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace L2_DAVH_AFPE.Models.Data
{
    public class Tree<T>
    {
        //public TreeNode<T> Root { get; set; }

        private TreeNode<T> root;        
        

        public Tree()
        {
            root = null;
        }

        internal TreeNode<T> Root { get => root; set => root = value; }

        public TreeNode<T> Insert(T newvalue, TreeNode<T> pNode, Func<T, bool> Comparer)
        {
            TreeNode<T> temp = null;

            if (pNode == null)
            { 
                temp = new TreeNode<T>(newvalue);

                return temp;
            }

            if (Comparer.Invoke(newvalue)) {
                pNode.left = Insert(newvalue, pNode.left, Comparer);
            }
            else
            {
                pNode.right = Insert(newvalue, pNode.right, Comparer);
            }
            return pNode;
        }
    }
}
