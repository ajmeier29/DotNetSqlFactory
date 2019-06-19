using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetSqlFactory.DataOperations
{
    public class SqlHelper<T>
    {
        private SqlDbType _sqlDbType;
        private List<T> _list;
        private string _patternValueName;
        private List<string> _parameterNameList;
        public SqlHelper(List<T> list, SqlDbType dbType, string patternValueName)
        {
            if(!patternValueName.StartsWith("@", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new ArgumentException("patternValueName must start with \'@\' ");
            }
            _list = list;
            _sqlDbType = dbType;
            _patternValueName = patternValueName;
            _parameterNameList = new List<string>();
        }

        public List<SqlParameter> ParameterInListHelper(int? size = null)
        {
            if((_sqlDbType == SqlDbType.VarChar || _sqlDbType == SqlDbType.NVarChar) && size == null)
            {
                throw new ArgumentException("size is null. When using SqlDbType.NVarChar or SqlDbType.VarChar you musts pass the variable size");
            }
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            for(int i = 0; i < _list.Count; i++)
            {
                // add to param list name
                _parameterNameList.Add(_patternValueName + i);
                if (size == null)  // only use db type and size for varchar, nvarchar
                {
                    sqlParameters.Add(new SqlParameter(
                        _patternValueName + i, _list.ElementAt(i)
                    ));
                } else
                {
                    SqlParameter sqlParameter = new SqlParameter(
                        _patternValueName + i, _sqlDbType, (int)size);
                    sqlParameter.Value = _list.ElementAt(i);
                    sqlParameters.Add(sqlParameter);
                }
            }
            return sqlParameters;
        }

        public string GenerateSqlCommandTextFromHelper(string stringToReplace, string sqlQuery)
        {
            return sqlQuery.Replace(stringToReplace, ParameterNames());
        }

        public string ParameterNames()
        {
            if(_parameterNameList.Count == 0)
            {
                throw new InvalidOperationException("_parameterNameList has no values but ParameterNames() is being called. _parameterNameList should be initialized first by calling a helper method.");
            }
            return string.Join(",", _parameterNameList.ToArray());
        }
    }
}
