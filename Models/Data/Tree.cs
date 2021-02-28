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

        public TreeNode<T> Insert(T newvalue, TreeNode<T> pNode, Func<T, int> Comparer)
        {
            TreeNode<T> temp = null;

            if (pNode == null)
            {
                
                temp = new TreeNode<T>(newvalue);
                return temp;
            }

            if (Comparer.Invoke(newvalue) > 0) {
                pNode.left = Insert(newvalue, pNode.left, Comparer);
            }
            else
            {
                pNode.right = Insert(newvalue, pNode.right, Comparer);
            }
            return pNode;
        }

        //public TreeNode<T> SearchParent(TreeNode<T> node, TreeNode<T> parent, Func<T, int> Comparer)
        //{
        //    TreeNode<T> temp = null;
        //    if (node == null || parent == null)
        //    {
        //        return null;
        //    }

        //    if (parent.right == node || parent.left == node)
        //    {
        //        return parent;
        //    }

        //    if(Comparer.Invoke(node.value) > 0)
        //    {

        //    }

        //}

        public void Delete()
        {

        }
    }
}
