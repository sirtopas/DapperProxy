using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace DapperProxy.DapperProxy
{
    /// <summary>
    /// A helper class that adds functionality to dapper. Use this class for all data access instead of calling dapper directly
    /// </summary>
    public class DapperProxy : IDapperProxy
    {
        private readonly string _connectionString;
        private readonly int _timeout;
        private string _storedProcedure;

        public DapperProxy(string connectionString, int timeout)
        {
            _connectionString = connectionString;
            _timeout = timeout;
        }

        public DynamicParameters Parameters { get; set; }

        public IDapperProxy AddParameter(string name, object parameter, DbType dbType, int? size = null, ParameterDirection? parameterDirection = 0)
        {
            if (Parameters == null)
            {
                Parameters = new DynamicParameters();
            }

            if (size.HasValue)
            {
                Parameters.Add(name, parameter, dbType, parameterDirection, size);
            }
            else
            {
                Parameters.Add(name, parameter, dbType, parameterDirection);
            }

            return this;
        }

        public IDapperProxy ClearParameters()
        {
            Parameters = null;

            return this;
        }

        public void Execute()
        {
            using (var connection = GetConnection())
            {
                connection.Execute(_storedProcedure, Parameters, commandType: CommandType.StoredProcedure, commandTimeout: _timeout);
            }
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TReturn>(Func<TFirst, TSecond, TThird, TReturn> map, string splitOn)
        {
            using (var connection = GetConnection())
            {
                var result = connection.Query(_storedProcedure, map, Parameters, splitOn: splitOn, commandType: CommandType.StoredProcedure, commandTimeout: _timeout);
                return result;
            }
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, string splitOn)
        {
            using (var connection = GetConnection())
            {
                var result = connection.Query(_storedProcedure, map, Parameters, splitOn: splitOn, commandType: CommandType.StoredProcedure, commandTimeout: _timeout);
                return result;
            }
        }

        public IDapperProxy WithStoredProcedure(string storedProcedure)
        {
            _storedProcedure = storedProcedure;

            return this;
        }

        private IDbConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
