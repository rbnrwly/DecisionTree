using System;

namespace DecisionTree.Data
{
    [Serializable]
    public class DataContextException : Exception
    {
        public DataContextException()
        { }

        public DataContextException(string message) : base(message)
        { }

        public DataContextException(string message, Exception innerException) : base(message, innerException)
        { }
    }
}