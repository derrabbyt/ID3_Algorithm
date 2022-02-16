using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ID3_Algorithm
{
    public class Node<T>
    {
        public Node(Type attr)
        {
            Attribute = attr;
        }
        public Node()
        {
        }
        public Type Attribute { get; set; }
        public List<Child<T>> Children { get; set; } = new List<Child<T>>();
        public object Result { get; set; }
        public object Evaluate(Enum[] day)
        {
            if (Children.Count == 0)
                return Result;
            var tempDay = day.ToList();
            foreach (var child in Children)
                foreach (var attr in day)
                    if (attr.ToString() == child.AttrType)
                    {
                        tempDay.Remove(attr);
                        if (child.NextNode == null)
                            return child.Result;
                        else
                            return child.NextNode.Evaluate(tempDay.ToArray());
                    }
            throw new Exception("No sufficient result could be found for the given Parameters!");
        }
    }
}
