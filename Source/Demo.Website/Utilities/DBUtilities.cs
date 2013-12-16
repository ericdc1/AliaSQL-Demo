using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Demo.Website.Utilities
{
    using System.Configuration;
    using System.Data.Common;
    using System.Data.SqlClient;

    using StackExchange.Profiling;
    using StackExchange.Profiling.Data;

    public class Database
    {
        public static DbConnection GetProfiledOpenConnection()
        {
            var sqlconnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DemoConnectionString"].ConnectionString);
            sqlconnection.Open();
            // wrap the connection with a profiling connection that tracks timings    
            return new ProfiledDbConnection(sqlconnection, MiniProfiler.Current);
        }

    }
}