using System.Collections.Generic;
using System.Linq;

namespace DecisionTree.Structure
{
    internal class Node
    {
        public string TargetAttribute { get; private set; }

        public Dictionary<string, Node> Children { get; private set; } = new Dictionary<string, Node>();

        public LeafNodeData LeafNodeData { get; private set; }

        public bool IsLeaf { get; private set; } = false;

        private double gain;

        private int dataSetCount;

        public Node(string targetAttribute, double gain, int dataSetCount)
        {
            TargetAttribute = targetAttribute;
            this.dataSetCount = dataSetCount;
            this.gain = gain;
        }

        public Node(string label, string reason, int dataSetCount)
        {
            LeafNodeData = new LeafNodeData(label, reason);
            this.dataSetCount = dataSetCount;
            IsLeaf = true;
        }

        public override string ToString()
        {
            if (IsLeaf)
            {
                return $"#[{LeafNodeData.Label}]";
            }
            else
            {
                return TargetAttribute;
            }
        }

        public string ToLongString()
        {
            if (IsLeaf)
            {
                return $"{LeafNodeData.Label} | {LeafNodeData.Reason} | {dataSetCount}";
            }
            else
            {
                return $"{TargetAttribute} | {gain:0.000} | {dataSetCount}";
            }
        }

        public void AddChild(string attributeValue, Node node)
        {
            Children.Add(attributeValue, node);
        }
        
        public string MostCommonTrainingValue()
        {
            int max = int.MinValue;
            string value = Children.Keys.FirstOrDefault();

            foreach (var child in Children)
            {
                if (child.Value.dataSetCount > max)
                {
                    value = child.Key;
                    max = child.Value.dataSetCount;
                }
            }

            return value;
        }

        public Node NextNode(string attributeValue)
		{
            if (Children.ContainsKey(attributeValue))
            {
                return Children[attributeValue];
            }
            else
            {
                throw new TreeNavigationException($"Unseen attribute value: {attributeValue}");
            }
		}
    }
}