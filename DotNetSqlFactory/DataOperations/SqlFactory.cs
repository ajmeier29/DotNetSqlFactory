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
        /// <summary>
        /// Closes the current DbConnection
        /// </summary>
        public void CloseConnection()
        {
            if(_dbConnection != null)
            {
                _dbConnection.Close();
            }
        }
        /// <summary>
        /// This query's main use case is to send a select query that uses a single 'IN' in the WHERE clause. You can use a 
        /// combination of the SqlHelper class to build the List<SqlParameter>, then call 
        /// SqlHelper.GenerateSqlCommandTextFromHelper to build out the query.
        /// </summary>
        /// <param name="sqlParameters">List of SqlParameters to be added to the DbCommand</param>
        /// <param name="customBuildQuery">This funtion shall be responsible for creating the DbCommand.CommandText. The @values should match the SqlParameters. Please see SqlHelper.cs for more information.</param>
        /// <returns></returns>
        public DataTable SqlSelectQuery(List<SqlParameter> sqlParameters,  Func<string> customBuildQuery)
        {
            OpenConnection();
            var dataTable = new DataTable();

            using (DbCommand command = _dbFactory.CreateCommand())
            {
                command.Connection = _dbConnection;
                command.CommandText = customBuildQuery();
                command.Parameters.AddRange(sqlParameters.ToArray());
                using (DbDataReader dataReader = command.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    dataTable.Load(dataReader);
                }
            }
            return dataTable;
        }
        /// <summary>
        /// This method makes a simple select query using the list of SqlParameters passed in. 
        /// </summary>
        /// <param name="sqlQuery">The SQL Query itself. Use @varibleName for each SqlParameter.</param>
        /// <param name="paramList">The list of SqlParameters to be injected into the sqlQuery.</param>
        /// <returns></returns>
        public DataTable SqlSelectQuery(string sqlQuery, List<SqlParameter> paramList)
        {
            OpenConnection();
            var dataTable = new DataTable();

            using (DbCommand command = _dbFactory.CreateCommand())
            {
                command.Connection = _dbConnection;
                command.CommandText = sqlQuery;
                command.Parameters.AddRange(paramList.ToArray());
                using (DbDataReader dataReader = command.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    dataTable.Load(dataReader);
                }
            }
            return dataTable;
        }
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="storedProcName"></param>
        /// <param name="paramList"></param>
        /// <returns></returns>
        public void StoredProcInsert(string storedProcName, List<SqlParameter> paramList)
        {
            OpenConnection();

            using (DbCommand command = _dbFactory.CreateCommand())
            {
                // create the command and its properties
                command.Connection = _dbConnection;
                command.CommandText = storedProcName;
                command.CommandType = CommandType.StoredProcedure;

                // add the input parameters
                command.Parameters.AddRange(paramList.ToArray());
                command.ExecuteNonQuery();
            }
            CloseConnection();
        }
    }
}
