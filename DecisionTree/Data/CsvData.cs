using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DecisionTree.Data
{
    public class CsvData : IData
    {
        public string AssignedClass { get; private set; } = string.Empty;

        private Dictionary<string, string> values = new Dictionary<string, string>();

        public CsvData(string valuesString, List<string> attributes, string assignedClass = "")
        {
            string[] valuesSplit = valuesString.Split(',');

            if (valuesSplit.Length != attributes.Count())
            {
                throw new DataContextException("Attribute and value count do not match");
            }

            if (valuesSplit.Length == 0)
            {
                throw new DataContextException("Values length is zero");
            }

            for (int i = 0; i < valuesSplit.Count(); i++)
            {
                values.Add(attributes[i], valuesSplit[i]);
            }

            AssignedClass = assignedClass;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(string.Join(", ", values.Select(attribute => attribute.Key + ":" + attribute.Value)));
            
            if (AssignedClass != string.Empty)
            {
                stringBuilder.Append(" | ");
                stringBuilder.Append(AssignedClass);
            }

            return stringBuilder.ToString();
        }

        public string GetAttributeValue(string attribute)
        {
            return values[attribute];
        }
    }
}