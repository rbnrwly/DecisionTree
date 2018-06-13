using Microsoft.VisualStudio.TestTools.UnitTesting;
using DecisionTree.Data;

namespace DecisionTreeTests
{
    [TestClass]
    public class CsvDataContextTests
    {
        [TestMethod]
        public void Create_simple_3_attribute_DataContext()
        {
            CsvDataContext dataContext = new CsvDataContext("a1,a2,a3");
            dataContext.AddData("v1,v2,v3", "c1");

            Assert.AreEqual("v1", dataContext.DataSet[0].GetAttributeValue("a1"));
            Assert.AreEqual("v2", dataContext.DataSet[0].GetAttributeValue("a2"));
            Assert.AreEqual("v3", dataContext.DataSet[0].GetAttributeValue("a3"));

            Assert.AreEqual("c1", dataContext.DataSet[0].AssignedClass);
        }

        [TestMethod]
        public void Parse_simple_3_attribute_CSV_data()
        {
            CsvDataContext dataContext = new CsvDataContext("a1,a2,a3");
            CsvData data = dataContext.Parse("v1,v2,v3");

            Assert.AreEqual("v1", data.GetAttributeValue("a1"));
            Assert.AreEqual("v2", data.GetAttributeValue("a2"));
            Assert.AreEqual("v3", data.GetAttributeValue("a3"));
        }

        [TestMethod]
        [ExpectedException(typeof(DataContextException))]
        public void Adding_data_with_too_many_attributes_should_throw_DataContextException()
        {
            CsvDataContext dataContext = new CsvDataContext("a1,a2,a3");
            dataContext.AddData("v1,v2,v3,v4", "c1");
        }

        [TestMethod]
        [ExpectedException(typeof(DataContextException))]
        public void Multiple_attributes_with_the_same_name_should_throw_DataContextException()
        {
            CsvDataContext dataContext = new CsvDataContext("a1,a2,a1");
        }
    }
}