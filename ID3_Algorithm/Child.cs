using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ID3_Algorithm
{
    public class Child<T>
    {
        public Child(string attrType)
        {
            AttrType = attrType;
        }
        public Node<T> NextNode { get; set; }
        public string AttrType { get; set; }
        public object Result { get; set; }

    }
}
