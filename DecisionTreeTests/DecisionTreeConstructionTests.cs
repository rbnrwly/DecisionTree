using Microsoft.VisualStudio.TestTools.UnitTesting;
using DecisionTree.Data;
using DecisionTree.Classification;

namespace DecisionTreeTests
{
    [TestClass]
    public class DecisionTreeConstructionTests
    {
        [TestMethod]
        public void Create_simple_single_layer_decision_tree()
        {
            CsvDataContext dataContext = new CsvDataContext("Attr1");

            dataContext.AddData("1", "Yes");
            dataContext.AddData("2", "No");

            DecisionTreeClassifier classifier = new DecisionTreeClassifier(dataContext.DataSet, dataContext.Attributes);

            Assert.AreEqual("(Attr1:1>Yes,2>No)", classifier.ToSingleLineString());

            Assert.AreEqual("Yes", classifier.Classify(dataContext.Parse("1")).AssignedClass);
            Assert.AreEqual("No", classifier.Classify(dataContext.Parse("2")).AssignedClass);

            Assert.AreEqual("Yes", classifier.Classify(dataContext.Parse("3"), false).AssignedClass);
            Assert.AreEqual("[No classification] - Classification failed", classifier.Classify(dataContext.Parse("3"), true).ToLongString());
        }

        [TestMethod]
        public void Create_simple_2_layer_decision_tree()
        {
            CsvDataContext dataContext = new CsvDataContext("Attr1,Attr2");

            dataContext.AddData("1,1", "Yes");
            dataContext.AddData("1,2", "No");
            dataContext.AddData("2,1", "Yes");
            dataContext.AddData("2,2", "No");

            DecisionTreeClassifier classifier = new DecisionTreeClassifier(dataContext.DataSet, dataContext.Attributes);

            Assert.AreEqual("(Attr2:1>Yes,2>No)", classifier.ToSingleLineString());

            Assert.AreEqual("Yes", classifier.Classify(dataContext.Parse("1,1")).AssignedClass);
            Assert.AreEqual("No", classifier.Classify(dataContext.Parse("1,2")).AssignedClass);

            Assert.AreEqual("Yes", classifier.Classify(dataContext.Parse("3,1")).AssignedClass);
            Assert.AreEqual("No", classifier.Classify(dataContext.Parse("3,2")).AssignedClass);

            Assert.IsTrue(classifier.Classify(dataContext.Parse("1,3")).ToLongString().Contains("Unseen value: 3 was substituted"));
        }
        
        [TestMethod]
        public void Create_simple_3_layer_decision_tree()
        {
            CsvDataContext dataContext = new CsvDataContext("Attr1,Attr2,Attr3");

            dataContext.AddData("1,1,3", "C1");
            dataContext.AddData("1,1,4", "C2");
            dataContext.AddData("1,2,3", "C2");
            dataContext.AddData("1,2,4", "C2");
            dataContext.AddData("2,1,3", "C3");
            dataContext.AddData("2,1,4", "C3");
            dataContext.AddData("2,2,3", "C4");
            dataContext.AddData("2,2,4", "C4");

            DecisionTreeClassifier classifier = new DecisionTreeClassifier(dataContext.DataSet, dataContext.Attributes);

            Assert.AreEqual("(Attr1:1(Attr2:1(Attr3:3>C1,4>C2),2>C2),2(Attr2:1>C3,2>C4))", classifier.ToSingleLineString());

            Assert.AreEqual("C1", classifier.Classify(dataContext.Parse("1,1,3")).AssignedClass);
            Assert.AreEqual("C2", classifier.Classify(dataContext.Parse("1,1,4")).AssignedClass);
            Assert.AreEqual("C3", classifier.Classify(dataContext.Parse("2,1,3")).AssignedClass);
            Assert.AreEqual("C4", classifier.Classify(dataContext.Parse("2,2,3")).AssignedClass);
        }

        [TestMethod]
        public void Create_complex_investment_example_decision_tree()
        {
            CsvDataContext dataContext = new CsvDataContext("Sector,Market,Compensation,Debt Ratio");

            dataContext.AddData("Tech,INT,Salary+Bonus,Low", "Yes");
            dataContext.AddData("Tech,UK,Salary,Low", "Yes");
            dataContext.AddData("Tech,INT,Equity,Low", "No");
            dataContext.AddData("Tech,US,Salary+Bonus,Med", "Yes");
            dataContext.AddData("Tech,US,Equity,Low", "Yes");
            dataContext.AddData("Health,UK,Salary+Bonus,Low", "Yes");
            dataContext.AddData("Health,INT,Salary+Bonus,High", "Yes");
            dataContext.AddData("Health,US,Salary,High", "No");
            dataContext.AddData("Health,EU,Equity,Low", "No");
            dataContext.AddData("Energy,UK,Salary+Bonus,Low", "Yes");
            dataContext.AddData("Energy,UK,Equity,Low", "No");
            dataContext.AddData("Energy,US,Salary,Med", "Yes");
            dataContext.AddData("Energy,EU,Salary,High", "No");
            dataContext.AddData("Energy,EU,Salary,Low", "Yes");
            dataContext.AddData("Media,EU,Salary,Low", "Yes");
            dataContext.AddData("Media,INT,Salary+Bonus,Low", "Yes");
            dataContext.AddData("Media,US,Equity,Med", "Yes");
            dataContext.AddData("Media,EU,Salary,High", "No");

            DecisionTreeClassifier classifier = new DecisionTreeClassifier(dataContext.DataSet, dataContext.Attributes);

            Assert.AreEqual("(Compensation:Salary+Bonus>Yes,Salary(Debt Ratio:Low>Yes,High>No,Med>Yes),Equity(Market:INT>No,US>Yes,EU>No,UK>No))", 
                            classifier.ToSingleLineString());

            Assert.AreEqual("Yes", classifier.Classify(dataContext.Parse("Tech,UK,Salary+Bonus,Low")).AssignedClass);

            Assert.AreEqual("Yes", classifier.Classify(dataContext.Parse("Energy,INT,Salary,Low")).AssignedClass);
            Assert.AreEqual("Yes", classifier.Classify(dataContext.Parse("Health,INT,Salary,Med")).AssignedClass);
            Assert.AreEqual("No", classifier.Classify(dataContext.Parse("Media,INT,Salary,High")).AssignedClass);

            Assert.AreEqual("No", classifier.Classify(dataContext.Parse("Tech,INT,Equity,Med")).AssignedClass);
            Assert.AreEqual("No", classifier.Classify(dataContext.Parse("Energy,UK,Equity,Low")).AssignedClass);
            Assert.AreEqual("Yes", classifier.Classify(dataContext.Parse("Health,US,Equity,High")).AssignedClass);
            Assert.AreEqual("No", classifier.Classify(dataContext.Parse("Media,EU,Equity,High")).AssignedClass);

            Assert.IsTrue(classifier.Classify(dataContext.Parse("Media,EU,Options,Zero")).ToLongString().Contains("Unseen value: Options was substituted"));
            Assert.IsTrue(classifier.Classify(dataContext.Parse("Media,EU,Options,Zero")).ToLongString().Contains("Unseen value: Zero was substituted"));
        }
    }
}