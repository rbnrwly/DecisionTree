using DecisionTree.Data;
using DecisionTree.Structure;
using System;
using System.Collections.Generic;
using System.Text;

namespace DecisionTree.Classification
{
    public class DecisionTreeClassifier
    {
        private Tree tree;

        private string treeAsMultiLineCachedString = string.Empty;

        private string treeAsSingleLineCachedString = string.Empty;

        public DecisionTreeClassifier(IEnumerable<IData> trainingData, IEnumerable<string> attributes, Action<string> logger = null)
        {
            tree = new Tree(trainingData, attributes, logger);
        }

        public ClassificationResult Classify(IData data, bool failOnUnseenValue = false)
        {
            try
            {
                ClassificationRunner classifier = new ClassificationRunner(data, tree.Root, failOnUnseenValue);
                classifier.Run();

                return classifier.ClassificationResult;
            }
            catch
            {
                return new ClassificationResult("Classification failed");
            }
        }

        public override string ToString()
        {
            if (treeAsMultiLineCachedString == string.Empty)
            {
                StringBuilder stringBuilder = new StringBuilder();
                WalkTreeToMultiLineString(tree.Root, stringBuilder);
                treeAsMultiLineCachedString = stringBuilder.ToString();
            }

            return treeAsMultiLineCachedString;
        }

        public string ToSingleLineString()
        {
            if (treeAsSingleLineCachedString == string.Empty)
            {
                StringBuilder stringBuilder = new StringBuilder();
                WalkTreeToSingleLineString(tree.Root, stringBuilder);
                treeAsSingleLineCachedString = stringBuilder.ToString();
            }

            return treeAsSingleLineCachedString;
        }

        private void WalkTreeToMultiLineString(Node currentNode, StringBuilder stringBuilder, int depth = 0)
        {
            stringBuilder.AppendLine(IndentString(currentNode.ToLongString(), depth));

            if (!currentNode.IsLeaf)
            {
                foreach (var node in currentNode.Children)
                {
                    stringBuilder.AppendLine(IndentString($"|{node.Key}", depth));
                    WalkTreeToMultiLineString(node.Value, stringBuilder, depth + 3);
                }
            }
        }

        private void WalkTreeToSingleLineString(Node currentNode, StringBuilder stringBuilder)
        {
            if (currentNode.IsLeaf)
            {
                stringBuilder.Append($">{currentNode.LeafNodeData.Label}");
            }
            else
            {
                stringBuilder.Append($"({currentNode.ToString()}:");
                foreach (var node in currentNode.Children)
                {
                    stringBuilder.Append($"{node.Key}");
                    WalkTreeToSingleLineString(node.Value, stringBuilder);
                    stringBuilder.Append(",");
                }
                stringBuilder.Length--;

                stringBuilder.Append(")");
            }
        }

        private string IndentString(string message, int depth = 0)
        {
            return $"{new string(' ', depth)}{message}";
        }
    }
 }