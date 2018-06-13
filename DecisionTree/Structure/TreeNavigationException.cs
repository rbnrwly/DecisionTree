using System;

namespace DecisionTree.Structure
{
    [Serializable]
    public class TreeNavigationException : Exception
    {
        public TreeNavigationException()
        { }

        public TreeNavigationException(string message) : base(message)
        { }

        public TreeNavigationException(string message, Exception innerException) : base(message, innerException)
        { }
    }
}