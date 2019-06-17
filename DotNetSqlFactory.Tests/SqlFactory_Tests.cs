using System;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DotNetSqlFactory.DataOperations;
using System.Data;

namespace DotNetSqlFactory.Tests
{
    [TestClass]
    public class SqlFactory_Tests
    {
        [TestMethod]
        public void SelectQuerySqlServer_Test()
        {
            var connectionstring = ConfigurationManager.AppSettings["connectionString"];
            SqlFactory sqlFactory = new SqlFactory(SqlFactory.DataProvider.SqlServer, connectionstring);
            sqlFactory.ExecuteQuery(@"select Make
                                        from dbo.Inventory
                                        where CarId = 8");
        }
        [TestMethod]
        public void SqlClientDbFactory_Test()
        {
            SqlFactory sqlFactory = new SqlFactory(SqlFactory.DataProvider.SqlServer);
            Assert.IsTrue(sqlFactory.DatabaseFactory.GetType().Name.Equals("SqlClientFactory"));
        }
        [TestMethod]
        public void OracleDbFactory_Test()
        {
            SqlFactory sqlFactory = new SqlFactory(SqlFactory.DataProvider.Oracle);
            Assert.IsTrue(sqlFactory.DatabaseFactory.GetType().Name.Equals("OracleClientFactory"));
        }

        [TestMethod]
        public void OledbDbFactory_Test()
        {
            SqlFactory sqlFactory = new SqlFactory(SqlFactory.DataProvider.OleDb);
            Assert.IsTrue(sqlFactory.DatabaseFactory.GetType().Name.Equals("OleDbFactory"));
        }

        [TestMethod]
        public void OdbcDbFactory_Test()
        {
            SqlFactory sqlFactory = new SqlFactory(SqlFactory.DataProvider.Odbc);
            Assert.IsTrue(sqlFactory.DatabaseFactory.GetType().Name.Equals("OdbcFactory"));
        }

    }
}
