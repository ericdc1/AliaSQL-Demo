namespace Demo.Website.Logic
{
    using System.Collections.Generic;

    using Models;

    public interface ITestTableLogic
    {
        IEnumerable<TestTable> GetList();
        TestTable GetItem(int id);
        TestTable SaveOrUpdate(TestTable entity);
        TestTable Delete(int id);
    }
}
