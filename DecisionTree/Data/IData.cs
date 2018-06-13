namespace DecisionTree.Data
{
    public interface IData
    {
        string AssignedClass { get; }

        string GetAttributeValue(string attribute);
    }
}