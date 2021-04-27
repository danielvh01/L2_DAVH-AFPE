using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataStructures
{
    public class BinaryTree<T> where T : IComparable
    {
        public TreeNode<T> Root { get; set; }
        public TreeNode<T> Work { get; set; }

        public int lenght = 0;
        public BinaryTree()
        {
            Root = null;
        }


        public TreeNode<T> Insert(T newvalue, TreeNode<T> pNode)
        {
            TreeNode<T> temp = null;
            if (pNode == null)
            {                
                temp = new TreeNode<T>(newvalue);
                if(lenght == 0)
                {
                    Root = temp;
                }
                lenght++;
                return temp;
            }
            if (newvalue.CompareTo(pNode.value) > 0) {
                pNode.left = Insert(newvalue, pNode.left);
            }
            else if(newvalue.CompareTo(pNode.value) < 0)
            {
                pNode.right = Insert(newvalue, pNode.right);
            }
            return pNode;
        }
        
        public T Find(Func<T, int> comparer, TreeNode<T> node)
        {
            if (node != null)
            {
                if (comparer.Invoke(node.value) == 0)
                {
                    return node.value;
                }
                if (comparer.Invoke(node.value) > 0)
                {
                    return Find(comparer, node.left);
                }
                else
                {
                    return Find(comparer, node.right);
                }
            }

            return default;
        }
        public TreeNode<T> SearchParent(TreeNode<T> node, TreeNode<T> parent)
        {
            TreeNode<T> temp = null;
            if (node == null)
            {
                return null;
            }
            if (parent.left != null)
            {
                if (parent.left.value.CompareTo(node.value) == 0)
                {
                    return parent;
                }
            }
            if (parent.right != null)
            {
                if (parent.right.value.CompareTo(node.value) == 0)
                {
                    return parent;
                }
            }

            if (node.value.CompareTo(parent.value) > 0 && parent.left != null)
            {
                temp = SearchParent(node, parent.left);
            }
            if(node.value.CompareTo(parent.value) < 0 && parent.right != null)
            {
                temp = SearchParent(node, parent.right); 
            }
            return temp;
        }

        public TreeNode<T> Delete(TreeNode<T> node, T value)
        {
            if(node == null)
            {
                return node;
            }
            if(value.CompareTo(node.value) > 0)
            {
                node.left = Delete(node.left, value);
            }
            else if(value.CompareTo(node.value) < 0)
            {
                node.right = Delete(node.right, value);
            }
            else
            {
                if (node.left == null)
                {
                    return node.right;
                }
                else if (node.right == null)
                {
                    return node.left;
                }
                else
                {
                    node.value = FindMinimum(node.right).value;
                    node.right = Delete(node.right, node.value);
                }
            }
            return node;
        }

        public TreeNode<T> FindMinimum(TreeNode<T> node)
        {
            TreeNode<T> minv = node;
            while (node.left != null)
            {
                node = node.left;
            }
            return minv;

        }

        
    }
}
