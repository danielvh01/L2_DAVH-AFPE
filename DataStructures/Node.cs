﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructures
{
    public class Node<T>
    {
        public Node<T> next = null;
        public Node<T> prev = null;
        public T value;
    }
}
