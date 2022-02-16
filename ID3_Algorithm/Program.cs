using System;
using System.Collections.Generic;
using System.Linq;

namespace ID3_Algorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            Enum[][] data = new Enum[][]
            {
                new Enum[] { Outlook.Sunny, Temp.Hot, Humidity.High, Wind.Weak, Play.No },
                new Enum[] { Outlook.Sunny, Temp.Hot, Humidity.High, Wind.Strong, Play.No },
                new Enum[] { Outlook.Overcast, Temp.Hot, Humidity.High, Wind.Weak, Play.Yes },
                new Enum[] { Outlook.Rain, Temp.Mild, Humidity.High, Wind.Weak, Play.Yes },
                new Enum[] { Outlook.Rain, Temp.Cold, Humidity.Normal, Wind.Weak, Play.Yes },
                new Enum[] { Outlook.Rain, Temp.Cold, Humidity.Normal, Wind.Strong, Play.No },
                new Enum[] { Outlook.Overcast, Temp.Cold, Humidity.Normal, Wind.Weak, Play.Yes },
                new Enum[] { Outlook.Sunny, Temp.Mild, Humidity.High, Wind.Weak, Play.No },
                new Enum[] { Outlook.Sunny, Temp.Cold, Humidity.Normal, Wind.Weak, Play.Yes },
                new Enum[] { Outlook.Rain, Temp.Mild, Humidity.Normal, Wind.Strong, Play.Yes },
                new Enum[] { Outlook.Sunny, Temp.Mild, Humidity.Normal, Wind.Strong, Play.Yes },
                new Enum[] { Outlook.Overcast, Temp.Mild, Humidity.High, Wind.Strong, Play.Yes },
                new Enum[] { Outlook.Overcast, Temp.Hot, Humidity.Normal, Wind.Weak, Play.Yes },
                new Enum[] { Outlook.Rain, Temp.Mild, Humidity.High, Wind.Strong, Play.No }
            };

            var tree = new Tree();
            var rootNode = tree.Train<Play>(data);
            object result = rootNode.Evaluate(new Enum[] { Outlook.Rain, Temp.Mild, Wind.Strong, Humidity.Normal });
            Console.WriteLine(result);
        }
    }
}
