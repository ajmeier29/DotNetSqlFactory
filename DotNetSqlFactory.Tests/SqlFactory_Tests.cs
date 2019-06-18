﻿using System;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DotNetSqlFactory.DataOperations;
using System.Data;
using System.Collections.Generic;

namespace DotNetSqlFactory.Tests
{
    [TestClass]
    public class SqlFactory_Tests
    {
        [TestMethod]
        public void SelectQuerySqlServer_Test()
        {
            var connectionstring = ConfigurationManager.AppSettings["connectionString"];
            SqlFactory<Inventory> sqlFactory = new SqlFactory<Inventory>(SqlFactory<Inventory>.DataProvider.SqlServer, connectionstring);
            List<Inventory> inventoryList = sqlFactory.QueryToList(@"SELECT [CarId]
                                      ,[Make]
                                      ,[Color]
                                      ,[PetName]
                                  FROM [BookLearning].[dbo].[Inventory]");
            Assert.IsTrue(inventoryList.Count == 7);
        }
        //[TestMethod]
        //public void SqlClientDbFactory_Test()
        //{
        //    SqlFactory sqlFactory = new SqlFactory(SqlFactory.DataProvider.SqlServer);
        //    Assert.IsTrue(sqlFactory.DatabaseFactory.GetType().Name.Equals("SqlClientFactory"));
        //}
        //[TestMethod]
        //public void OracleDbFactory_Test()
        //{
        //    SqlFactory sqlFactory = new SqlFactory(SqlFactory.DataProvider.Oracle);
        //    Assert.IsTrue(sqlFactory.DatabaseFactory.GetType().Name.Equals("OracleClientFactory"));
        //}

        //[TestMethod]
        //public void OledbDbFactory_Test()
        //{
        //    SqlFactory sqlFactory = new SqlFactory(SqlFactory.DataProvider.OleDb);
        //    Assert.IsTrue(sqlFactory.DatabaseFactory.GetType().Name.Equals("OleDbFactory"));
        //}

        //[TestMethod]
        //public void OdbcDbFactory_Test()
        //{
        //    SqlFactory sqlFactory = new SqlFactory(SqlFactory.DataProvider.Odbc);
        //    Assert.IsTrue(sqlFactory.DatabaseFactory.GetType().Name.Equals("OdbcFactory"));
        //}
    }

    public class Inventory
    {
        public int CarId { get; set; }
        public string Make { get; set; }
        public string Color { get; set; }
        public string PetName { get; set; }
    }

}
