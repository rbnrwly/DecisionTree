using System.Collections.Generic;
using System.Linq;

namespace DecisionTree.Data
{
    public class CsvDataContext
    {
        public List<string> Attributes { get; private set; } = new List<string>();

        public List<CsvData> DataSet { get; private set; } = new List<CsvData>();

        public CsvDataContext(string attributes)
        {
            string[] attributesSplit = attributes.Split(',');

            if (attributesSplit.Count() == 0)
            {
                throw new DataContextException("No attributes specified");
            }

            foreach (string attribute in attributesSplit)
            {
                if (!Attributes.Contains(attribute))
                {
                    Attributes.Add(attribute);
                }
                else
                {
                    throw new DataContextException($"Multiple attributes with the same name: {attribute}");
                }

                if (attribute == string.Empty)
                {
                    throw new DataContextException("Attribute name is empty");
                }
            }
        }

        public void AddData(string values, string assignedClass)
        {
            DataSet.Add(new CsvData(values, Attributes, assignedClass));
        }

        public CsvData Parse(string values)
        {
            return new CsvData(values, Attributes);
        }
    }
}