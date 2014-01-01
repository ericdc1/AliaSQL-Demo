using System.Text;
using Demo.SeleniumTests.PageObjects.Basic;
using Demo.SeleniumTests.Utilities;
using NUnit.Framework;

namespace Demo.SeleniumTests.Tests
{
    public class DemoPage
    {
        private StringBuilder verificationErrors;
        [TestFixtureSetUp]
        public void SetupTest()
        {
            Browser.StartWebDriver().Navigate().Refresh();
            BasicPageActions.GoTo();
            verificationErrors = new StringBuilder();
        }

        [Test]
        public void CanGoToDemoPage()
        {
            Assert.IsTrue(BasicPageActions.IsAt());
        }

        [Test]
        public void CanAddNewRecord()
        {
            BasicPageActions.AddNewRecord();
            Assert.IsTrue(BasicPageActions.WasNewRecordAdded());
        }

        [Test]
        public void CanDeleteRecord()
        {
            BasicPageActions.DeleteRecord();
            Assert.IsTrue(BasicPageActions.WasRecordDeleted());
        }

        //TestFixtureTearDown can be commented out if there are are a series of tests to be run, it should only be on the final test in the suite.
        [TestFixtureTearDown]
        public void TearDownTest()
        {
            BasicPageActions.StopTests();
            Assert.AreEqual("", verificationErrors.ToString());
        }

    }
}
