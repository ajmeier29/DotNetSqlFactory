using System;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DotNetSqlFactory.DataOperations;

namespace DotNetSqlFactory.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void SqlClientAppConfigValidation()
        {
            var provider = ConfigurationManager.AppSettings["provider"];
            SqlFactory sqlFactory = new SqlFactory(provider);
        }
    }
}
