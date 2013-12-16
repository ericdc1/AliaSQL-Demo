namespace Demo.Website.Logic.Implementations
{
    using System.Collections.Generic;

    using Models;
    using RepositoryInterfaces;

    public class TestTableLogic : ITestTableLogic
    {
        private readonly ITestTableRepository _testTableRepository;
        public TestTableLogic(ITestTableRepository testTableRepository)
        {
            _testTableRepository = testTableRepository;
        }

        public IEnumerable<TestTable> GetList()
        {
            return _testTableRepository.GetList();
        }

        public TestTable GetItem(int id)
        {
            return _testTableRepository.GetItem(id);
        }

        public TestTable SaveOrUpdate(TestTable entity)
        {
            if (entity.CalculatedTotal < 10)
                return entity.Id > 0 ? _testTableRepository.Update(entity) : _testTableRepository.Insert(entity);

            entity.Errors.Add(BaseModel.GetPropertyName(() => entity.value1), "Total must be less than 10");
            return entity;
        }

        public TestTable Delete(int id)
        {
            var record = _testTableRepository.GetItem(id);
            return _testTableRepository.Delete(record);
        }
    }
}