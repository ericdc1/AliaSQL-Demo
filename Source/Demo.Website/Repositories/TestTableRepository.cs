namespace Demo.Website.Repositories
{
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Linq;

    using Dapper;

    using Demo.Website.Models;
    using Demo.Website.RepositoryInterfaces;

    public class TestTableRepository : ITestTableRepository
    {
        private DbConnection connection;

        public IEnumerable<TestTable> GetList()
        {
            using (this.connection = Utilities.Database.GetProfiledOpenConnection())
            {
                var result = this.connection.GetList<TestTable>();
                return result.ToList();
            }
        }

        public TestTable GetItem(int id)
        {
            using (this.connection = Utilities.Database.GetProfiledOpenConnection())
            {
                return this.connection.Get<TestTable>(id);
            }
        }

        public TestTable Insert(TestTable entity)
        {
            using (this.connection = Utilities.Database.GetProfiledOpenConnection())
            {
                var insert = this.connection.Insert(entity);
                entity.Id = (int)insert;
                return entity;
            }
        }

        public TestTable Update(TestTable entity)
        {
            using (this.connection = Utilities.Database.GetProfiledOpenConnection())
            {
                this.connection.Update(entity);
            }
            return entity;
        }

    
        public TestTable Delete(TestTable entity)
        {
            using (this.connection = Utilities.Database.GetProfiledOpenConnection())
            {
                this.connection.Delete(entity);
            }
            return entity;
        }

    }
}