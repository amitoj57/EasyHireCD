using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using EasyHireCD;

namespace EasyHireCD_UnitTestingProject
{
    [TestClass]
    public class EasyHireCD_UnitTestClass
    {
        DatabaseManager Data_Obj = new DatabaseManager();
        [TestMethod]
        public void MovieCountUnitTest()
        {
            int total_title_count = Data_Obj.CheckName("Being There");
            Assert.AreEqual(1, total_title_count);
        }
        [TestMethod]
        public void MovieCountUnitTest_Neg()
        {
            int total_title_count = Data_Obj.CheckName("Being There");
            Assert.AreNotEqual(5, total_title_count);
        }
    }
}
