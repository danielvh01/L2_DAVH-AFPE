using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace L2_DAVH_AFPE.Models.Data
{
    public class Tree
    {
        //public TreeNode<T> Root { get; set; }

        private TreeNode root;        
        

        public Tree()
        {
            root = null;
        }

        internal TreeNode Root { get => root; set => root = value; }

        public TreeNode Insert(int ID, TreeNode pNode)
        {
            TreeNode temp = null;

            if (pNode == null)
            { 
                temp = new TreeNode();
                temp.Id = ID;

                return temp;
            }

            if (ID < pNode.Id) {
                pNode.left = Insert(ID,pNode.left);
            }
            if (ID > pNode.Id)
            {
                pNode.right = Insert(ID, pNode.right);
            }
            return pNode;
        }
    }
}
