using System;
using System.Collections.Generic;
using System.Text;

namespace L2_DAVH_AFPE.Models.Data
{
    public class TreeNode
    {
        public int id;
        public TreeNode left = null;
        public TreeNode right = null;

        public int Id { get => id; set => id = value; }

        internal TreeNode Left { get => left; set => left = value; }
        internal TreeNode Right { get => right; set => right = value; }

        public TreeNode()
        {
            id = default;
            left = null;
            right = null;
        }


        
       
    }
}
