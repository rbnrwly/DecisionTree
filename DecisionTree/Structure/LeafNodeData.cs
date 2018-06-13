namespace DecisionTree.Structure
{
    internal class LeafNodeData
    {
        public string Label { get; private set; } = string.Empty;

        public string Reason { get; private set; } = string.Empty;

        public LeafNodeData(string label, string reason)
        {
            Label = label;
            Reason = reason;
        }
    }
}