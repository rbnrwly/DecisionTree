using DecisionTree.Data;
using DecisionTree.Structure;

namespace DecisionTree.Classification
{
    internal class ClassificationRunner
    {
        public ClassificationResult ClassificationResult { get; private set; } = new ClassificationResult();

        private bool failOnUnseenValue;

        private IData data;

        private Node rootNode;

        public ClassificationRunner(IData data, Node rootNode, bool failOnUnseenValue)
        {
            this.failOnUnseenValue = failOnUnseenValue;
            this.rootNode = rootNode;
            this.data = data;
        }

        public void Run()
        {
            ClassifyData(rootNode);
        }

        private void ClassifyData(Node currentNode)
        {
            if (!currentNode.IsLeaf)
            {
                string attributeValue = data.GetAttributeValue(currentNode.TargetAttribute);

                if (failOnUnseenValue)
                {
                    ClassifyData(currentNode.NextNode(attributeValue));
                }
                else
                {
                    try
                    {
                        ClassifyData(currentNode.NextNode(attributeValue));
                    }
                    catch (TreeNavigationException)
                    {
                        string mostCommonTrainingValue = currentNode.MostCommonTrainingValue();
                        ClassificationResult.Log($"Unseen value: {attributeValue} was substituted as: {mostCommonTrainingValue}");

                        ClassifyData(currentNode.NextNode(mostCommonTrainingValue));
                    }
                }
            }
            else
            {
                ClassificationResult.SetAssignedClass(currentNode.LeafNodeData.Label);
            }
        }
    }
}