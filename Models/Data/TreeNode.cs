using System;
using System.Collections.Generic;
using System.Text;

namespace L2_DAVH_AFPE.Models.Data
{
    public class TreeNode<T>
    {
        public TreeNode<T> parent = null;
        public TreeNode<T> left = null;
        public TreeNode<T> right = null;
        public T value { get; set; }


        internal TreeNode<T> Left { get => left; set => left = value; }
        internal TreeNode<T> Right { get => right; set => right = value; }
        internal TreeNode<T> Parent { get => parent; set => parent = value; }

        public TreeNode(T newvalue)
        {
            value = newvalue;
            left = null;
            right = null;
            parent = null;
        }


        
       
    }
}
