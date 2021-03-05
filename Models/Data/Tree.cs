﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace L2_DAVH_AFPE.Models.Data
{
    public class Tree<T> where T : IComparable
    {
        public TreeNode<T> Root { get; set; }
        public TreeNode<T> Work { get; set; }

        public int lenght = 0;
        public Tree()
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
            if (newvalue.CompareTo(pNode.value) < 0) {
                pNode.left = Insert(newvalue, pNode.left);
            }
            else if(newvalue.CompareTo(pNode.value) > 0)
            {
                pNode.right = Insert(newvalue, pNode.right);
            }
            return pNode;
        }
        public TreeNode<T> Find(T value, TreeNode<T> node)
        {
            if (node != null)
            {
                if (value.CompareTo(node.value) == 0)
                {
                    return node;
                }
                if (value.CompareTo(node.value) < 0)
                {
                    return Find(value, node.left);
                }
                else
                {
                    return Find(value, node.right);
                }
            }

            return null;
        }
        public TreeNode<T> SearchParent(TreeNode<T> node, TreeNode<T> parent)
        {
            TreeNode<T> temp = null;
            if (node == null || parent == null)
            {
                return null;
            }

            if (parent.right == node || parent.left == node)
            {
                return parent;
            }

            if (parent.value.CompareTo(node.value) < 0 && parent.left != null)
            {
                temp = SearchParent(node, parent.left);
            }
            if(parent.value.CompareTo(node.value) > 0 && parent.right != null)
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
            if(value.CompareTo(node.value) < 0)
            {
                node.left = Delete(node.left, value);
            }
            else if(value.CompareTo(node.value) > 0)
            {
                node.right = Delete(node.right, value);
            }
            else
            {
                if(node.left == null && node.right == null)
                {
                    node = null;
                    return node;
                }
                else if(node.left == null)
                {
                    TreeNode<T> parent = SearchParent(node, Root);
                    parent.right = node.right;
                    return node;
                }
                else if(node.right == null)
                {
                    TreeNode<T> parent = SearchParent(node, Root);
                    parent.left = node.left;
                    return node;
                }
                else
                {
                    TreeNode<T> minimo = null;
                    while(node.left != null)
                    {
                        minimo = node.left;
                    }
                    node.value = minimo.value;
                    node.right = Delete(node.right, minimo.value);
                    return node;
                }
            }
            return node;
        }

        public int FindMinimum(TreeNode<T> node)
        {
            if (node == null)
            {
                return 0;
            }
            Work = node;
            int minimum = Work.value;

            while (Work.left != null)
            {
                Work = Work.left;
                minimum = Work.value;
            }
            return minimum;

        }

        
    }
}
