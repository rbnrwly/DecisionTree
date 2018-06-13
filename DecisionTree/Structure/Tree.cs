using DecisionTree.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DecisionTree.Structure
{
    internal class Tree
    {
        internal Node Root { get; private set; }

        private IEnumerable<string> attributes;

        private Action<string> logger;

		public Tree(IEnumerable<IData> trainingData, IEnumerable<string> attributes, Action<string> logger = null)
		{
            Log("Starting decision tree construction");

            this.logger = logger;
            this.attributes = attributes;
            Root = CreateTree(trainingData, new HashSet<string>());
        }
          
        private Node CreateTree(IEnumerable<IData> dataSet, HashSet<string> usedAttributes, int depth = 0)
		{
            if (AllSameClass(dataSet))
            {
                string sameClass = dataSet.First().AssignedClass;

                Log($"#[{sameClass}] All remaining samples ({dataSet.Count()}) in this class", depth);

                return new Node(sameClass, "All remaining samples in same class", dataSet.Count());
            }
            else if (UsedAllAttributes(usedAttributes))
            {
                string mostCommonClassRemaining = MostCommonClass(dataSet);

                Log($"#[{mostCommonClassRemaining}] Full mask", depth);

                return new Node(mostCommonClassRemaining, "Full mask", dataSet.Count());
            }
            else
            {
                Dictionary<string, double> gains;
                string bestAttribute = GetBestAttribute(dataSet, attributes.Except(usedAttributes), out gains);

                Node node = new Node(bestAttribute, gains[bestAttribute], dataSet.Count());
                usedAttributes.Add(bestAttribute);

                Log($"{bestAttribute} is the best attribute from: {string.Join(", ", gains.Select(a => a.Key + ":" + a.Value.ToString("0.000")))}", depth);
                
                foreach (var subSet in dataSet.GroupBy(data => data.GetAttributeValue(bestAttribute)))
                {
                    Log($"| {subSet.Key}", depth);

                    HashSet<string> copyOfUsedAttributes = new HashSet<string>(usedAttributes);
                    node.AddChild(subSet.Key, CreateTree(subSet, copyOfUsedAttributes, depth + 3));
                }

                return node;
            }
		}

        private string GetBestAttribute(IEnumerable<IData> dataSet, IEnumerable<string> availableAttributes, out Dictionary<string, double> gains)
        {
            Dictionary<string, double> attributeGains = new Dictionary<string, double>();

            double bestGain = double.MinValue;
            string bestAttribute = availableAttributes.First();

            foreach (string attribute in availableAttributes)
            {
                double gain = Gain(dataSet, attribute);

                attributeGains.Add(attribute, gain);

                if (gain > bestGain)
                {
                    bestAttribute = attribute;
                    bestGain = gain;
                }
            }

            gains = attributeGains;

            return bestAttribute;
        }

        private double Gain(IEnumerable<IData> dataSet, string attribute)
        {
            double totalEntropy = 0;

            foreach (var subSet in dataSet.GroupBy(data => data.GetAttributeValue(attribute)))
            {
                double proportion = (double)subSet.Count() / dataSet.Count();

                totalEntropy += proportion * Entropy(subSet);
            }

            return Entropy(dataSet) - totalEntropy;
        }

        private double Entropy(IEnumerable<IData> dataSet)
        {
            double entropy = 0;

            foreach (var subSet in dataSet.GroupBy(data => data.AssignedClass))
            {
                double proportion = (double)subSet.Count() / dataSet.Count();
                double logProportion = Math.Log(proportion, 2); // Base 2 gives bits of entropy

                entropy -= proportion * logProportion;
            }

            return entropy;
        }

        private string MostCommonClass(IEnumerable<IData> dataSet)
        {
            return dataSet.GroupBy(c => c.AssignedClass).OrderByDescending(g => g.Count()).Take(1).Select(g => g.Key).First();
        }

        private bool AllSameClass(IEnumerable<IData> dataSet)
        {
            return (dataSet.GroupBy(c => c.AssignedClass).Count() == 1);
        }

        private bool UsedAllAttributes(HashSet<string> usedAttributes)
        {
            return (usedAttributes.Count() == attributes.Count());
        }

        private void Log(string message, int depth = 0)
        {
            logger?.Invoke($"{new string(' ', depth)}{message}");
        }
    }
}