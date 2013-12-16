using System;
using Demo.Website.Logic;
using Demo.Website.Logic.Implementations;
using Demo.Website.RepositoryInterfaces;

namespace Demo.UnitTests
{

    using Moq;
    using Xunit;

    public class DemoTest : IDisposable
    {
        private ITestTableLogic _testTableLogic;
        private readonly Mock<ITestTableRepository> _mockTestTableRepository;

        public DemoTest()
        {           
            _mockTestTableRepository = new Mock<ITestTableRepository>();
            _testTableLogic = new TestTableLogic(_mockTestTableRepository.Object);
        }


        [Fact]
        public void NewRecordAddsValues()
        {
            var testrecord = new Website.Models.TestTable();
            testrecord.value1 = 3;
            testrecord.value2 = 5;
            _mockTestTableRepository.Setup(f => f.Insert(It.IsAny<Website.Models.TestTable>())).Returns(testrecord);
            var result = _testTableLogic.SaveOrUpdate(testrecord);
            Assert.True(result.CalculatedTotal == 8);
        }



        [Fact]
        public void NewRecordDoesNotAddsValuesWhenTotalOver10()
        {
            var testrecord = new Website.Models.TestTable();
            testrecord.value1 = 9;
            testrecord.value2 = 5;
            _mockTestTableRepository.Setup(f => f.Insert(It.IsAny<Website.Models.TestTable>())).Returns(testrecord);
            var result = _testTableLogic.SaveOrUpdate(testrecord);
            Assert.True(result.Errors.ContainsValue("Total must be less than 10"));
        }


        public void Dispose()
        {
            _testTableLogic = null;
        }
    }
}
