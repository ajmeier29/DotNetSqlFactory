using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;

namespace DotNetSqlFactory.DataOperations
{
    public class SqlFactory
    {
        DbConnection _dbConnection;
        private string _provider;
        public SqlFactory(string provider)
        {
            _provider = provider;
        }
    }
}
