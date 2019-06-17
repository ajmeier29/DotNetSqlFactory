using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

// different providers
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.OracleClient;
using System.Data.OleDb;
using System.Data.Odbc;

namespace DotNetSqlFactory.DataOperations
{
    public class SqlFactory
    {
        DbConnection _dbConnection;
        public DbConnection DatabaseConnection
        {
            get => _dbConnection;
            set => _dbConnection = value;
        }

        DbProviderFactory _dbFactory;
        public DbProviderFactory DatabaseFactory
        {
            get => _dbFactory;
            set => _dbFactory = value;
        }
        private string _connectionString;

        public string ConnectionString
        {
            get => _connectionString;
            set => _connectionString = value;
        }

        private string _provider;
        public enum DataProvider
        {
            SqlServer,
            Oracle,
            OleDb,
            Odbc,
            None
        }

        public SqlFactory(DataProvider dataProvider, string connectionString)
        {
            _connectionString = connectionString;
            SetFactory(dataProvider);
        }

        public SqlFactory(DataProvider dataProvider)
        {
            SetFactory(dataProvider);
        }

        /// <summary>
        /// Returns the appropriate DbProviderFactory for the DataProvider passed in. 
        /// </summary>
        /// <param name="dataProvider"></param>
        /// <returns></returns>
        public void SetFactory(DataProvider dataProvider)
        {
            switch (dataProvider)
            {
                case DataProvider.SqlServer:
                    _dbFactory = DbProviderFactories.GetFactory("System.Data.SqlClient");
                    break;
                case DataProvider.Oracle:
                    _dbFactory = DbProviderFactories.GetFactory("System.Data.OracleClient");
                    break;
                case DataProvider.OleDb:
                    _dbFactory = DbProviderFactories.GetFactory("System.Data.OleDb");
                    break;
                case DataProvider.Odbc:
                    _dbFactory = DbProviderFactories.GetFactory("System.Data.Odbc");
                    break;
                default:
                    var providerList = Enum.GetValues(typeof(DataProvider));
                    throw new Exception($"Invalid DataProvider. Accepted values: {providerList}");
            }
        }
        /// <summary>
        /// Creates a connection from the current DatabaseFactory
        /// </summary>
        /// <returns></returns>
        private  DbConnection CreateConnection()
        {
            return _dbFactory.CreateConnection();
        }
        /// <summary>
        /// Opens a connection with the active DatabaseConnection. If DatabaseConnection is null, it will try to create a
        /// new one from the current DatabaseFactory
        /// </summary>
        public void OpenConnection()
        {
            if(_dbConnection == null)
            {
                // try to create a connection if one has not been created
                try
                {
                    _dbConnection = CreateConnection();
                }
                catch (NullReferenceException ex)
                {
                    throw new Exception("DatabaseFactory is null!! Seta DatabaseFactory before trying to open a connection!");
                }
            }
            _dbConnection.ConnectionString = _connectionString; 
            _dbConnection.Open();
        }

        public void ExecuteQuery(string sqlQuery)
        {
            OpenConnection();

            using(DbCommand command = _dbFactory.CreateCommand())
            {
                command.Connection = _dbConnection;
                command.CommandText = sqlQuery;
                using(DbDataReader dataReader = command.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (dataReader.Read())
                    {
                        var test = dataReader["Make"];
                    }
                }
            }
        }

    }
}
