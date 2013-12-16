using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;

namespace Demo.Website.Models
{
    public class TestTable : Database.TestTable
    {
        [Editable(false)]
        public int CalculatedTotal
        {
            get
            {
                return value1 + value2;
            }
        }
    }
}