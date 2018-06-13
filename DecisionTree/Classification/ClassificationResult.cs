using System.Collections.Generic;

namespace DecisionTree.Classification
{
    public class ClassificationResult
    {
        public string AssignedClass { get; private set; } = string.Empty;

        public List<string> ClassificationLog { get; private set; } = new List<string>();

        public ClassificationResult()
        {
        }

        public ClassificationResult(string logMessage)
        {
            Log(logMessage);
        }

        public void SetAssignedClass(string assignedClass)
        {
            AssignedClass = assignedClass;
        }

        public void Log(string message)
        {
            ClassificationLog.Add(message);
        }

        public override string ToString()
        {
            return AssignedClass;
        }

        public string ToLongString()
        {
            string logString = string.Join(", ", ClassificationLog);
           
            if (AssignedClass != string.Empty)
            {
                return $"{AssignedClass} - {logString}";
            }
            else
            {
                return $"[No classification] - {logString}";
            }
        }
    }
}