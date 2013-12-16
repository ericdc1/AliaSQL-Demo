namespace Demo.Website.RepositoryInterfaces
{
    using System.Collections.Generic;

    using Demo.Website.Models;

    public interface ITestTableRepository
    {
        IEnumerable<TestTable> GetList();
        TestTable GetItem(int id);
        TestTable Insert(TestTable entity);
        TestTable Update(TestTable entity);
        TestTable Delete(TestTable entity);
    }
}
