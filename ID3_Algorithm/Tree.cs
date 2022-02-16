using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ID3_Algorithm
{
    class Tree
    {
        /// <summary>
        /// Gets the root node and builds the tree of the given traings data.
        /// </summary>
        /// <typeparam name="T">The label Enum/Type.</typeparam>
        /// <param name="data">The traings data (days).</param>
        /// <returns>The root node of the ID3Tree with all its children.</returns>
        public Node<T> Train<T>(Enum[][] data)
        {
            Node<T> rootNode = new Node<T>();
            var bestInformationGain = 0.0;
            var attributes = GetAttributesFromData<T>(data);
            foreach (var attributeValues in attributes)
            {
                var gain = CalcualteInformationGain(attributeValues);
                if (gain > bestInformationGain)
                {
                    bestInformationGain = gain;
                    rootNode.Attribute = attributeValues[0].Item1.GetType();
                }
            }
            rootNode = BuildTree(rootNode, data);
            return rootNode;
        }

        /// <summary>
        /// Recursive function for setting the children of the given node.
        /// </summary>
        /// <typeparam name="T">The label Enum/Type.</typeparam>
        /// <param name="Node">The node of which we want to set the children.</param>
        /// <param name="days">The days of a specific branch.</param>
        /// <returns></returns>
        private Node<T> BuildTree<T>(Node<T> Node, Enum[][] days)
        {
            Array attributeTypes = Enum.GetNames(Node.Attribute);

            foreach (var type in attributeTypes)
            {
                var child = new Child<T>(type.ToString());
                var subSet = ReduceDataSet(days, type);
                if (subSet.Count() == 1)
                {
                    child.Result = GetResult<T>(subSet.FirstOrDefault());
                    Node.Children.Add(child);
                    continue;
                }
                else if (subSet.Count() < 1)
                {
                    continue;
                }

                var attributes = GetAttributesFromData<T>(subSet).Where(a => a[0].Item1.GetType() != Node.Attribute).ToList();

                (Type attr, double bestInformationGain) = GetBestInformationGain(attributes.ToList());
                if (bestInformationGain == 0)
                {
                    child.Result = GetResult<T>(subSet.FirstOrDefault());
                    Node.Children.Add(child);
                    continue;
                }
                var nextNode = BuildTree<T>(new Node<T>(attr), subSet);
                child.NextNode = nextNode;
                Node.Children.Add(child);

            }
            return Node;
        }

        /// <summary>
        /// Evaluates the value of the result/label from a given day.
        /// </summary>
        /// <typeparam name="T">The Enum which represents the label.</typeparam>
        /// <param name="day">The day of which we want to know the outcome.</param>
        /// <returns></returns>
        private object GetResult<T>(Enum[] day)
        {
            foreach (var attr in day)
                if (attr.GetType() == typeof(T))
                    return attr;
            return null;
        }

        /// <summary>
        /// Reduces a given set of days to just days where a certain attribute has a certain type.
        /// </summary>
        /// <param name="days">The days which we want to reduce.</param>
        /// <param name="type">The attribute value of which we want the days.</param>
        /// <returns>The reduced days.</returns>
        private Enum[][] ReduceDataSet(Enum[][] days, object type)
        {
            List<Enum[]> reducedDays = new List<Enum[]>();
            foreach (var day in days)
                foreach (var attr in day)
                    if (attr.ToString() == type.ToString() && reducedDays.Contains(day) == false)
                    {
                        reducedDays.Add(day);
                        break;
                    }
            return reducedDays.ToArray();
        }

        /// <summary>
        /// Gets the attributes from a given list of days.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<List<(Enum, object)>> GetAttributesFromData<T>(Enum[][] data)
        {
            List<List<(Enum, object)>> attributes = new List<List<(Enum, object)>>();
            for (int i = 0; i < data[0].Length - 1; i++)
            {
                attributes.Add(new List<(Enum, object)>());
                foreach (var day in data.Where(d => d.GetType() != typeof(T)))
                {
                    attributes[i].Add((day[i], GetResult<T>(day)));
                }
            }
            return attributes;
        }

        /// <summary>
        /// Calculates the entropy for a list of attributes and the according result.
        /// </summary>
        /// <param name="attributeSample"></param>
        /// <returns></returns>
        public double CalculateEntropy(List<(Enum, object)> attributeSample)
        {
            double entropy = 0.0;
            if (attributeSample.Count != 0)
            {
                Array attributeTypes = Enum.GetNames(attributeSample[0].Item2.GetType());
                foreach (var attributeType in attributeTypes)
                {
                    var amountOfElementsInClass = 0.0;
                    foreach (var attribute in attributeSample)
                    {
                        if (attribute.Item2.ToString() == attributeType.ToString())
                            amountOfElementsInClass++;
                    }
                    if (amountOfElementsInClass != 0)
                    {
                        var portionOfClassInSample = amountOfElementsInClass / attributeSample.Count;
                        entropy += -portionOfClassInSample * Math.Log2(portionOfClassInSample);
                    }
                }
            }
            return entropy;
        }
        /// <summary>
        /// Calculates the informaton gain for a list of attributes and the according result.
        /// </summary>
        /// <param name="attributeSample"></param>
        /// <returns></returns>
        public double CalcualteInformationGain(List<(Enum, object)> attributeSample)
        {
            var entropy = CalculateEntropy(attributeSample);
            var information = 0.0;
            var amountOfelements = attributeSample.Count();

            Array attributeTypes = Enum.GetNames(attributeSample[0].Item1.GetType());
            foreach (var attributeType in attributeTypes)
            {
                var amountOfElementsInClass = 0.0;
                var elementsInClass = attributeSample.Where(a => a.Item1.ToString() == attributeType.ToString()).ToList();

                foreach (var attribute in attributeSample)
                {
                    if (attribute.Item1.ToString() == attributeType.ToString())
                        amountOfElementsInClass++;
                }

                var portionOfClassInSample = amountOfElementsInClass / amountOfelements;
                var entropyOfElementsInClass = CalculateEntropy(elementsInClass);
                information += portionOfClassInSample * entropyOfElementsInClass;
            }
            return entropy - information;
        }

        /// <summary>
        /// Gets the attribute with the highest information gain from a given list of attributes.
        /// </summary>
        /// <param name="Nodes"></param>
        /// <returns></returns>
        public (Type, double) GetBestInformationGain(List<List<(Enum, object)>> Nodes)
        {

            var highestInformationGain = 0.0;
            var bestNode = Nodes[0];

            foreach (var node in Nodes)
            {
                var informationGain = CalcualteInformationGain(node.ToList());
                if (informationGain > highestInformationGain)
                {
                    highestInformationGain = informationGain;
                    bestNode = node;
                }
            }
            return (bestNode[0].Item1.GetType(), highestInformationGain);
        }
    }

}
