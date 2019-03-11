using System;
using System.Collections.Generic;
using System.Data;
using Dapper;

namespace DapperProxy.DapperProxy
{
    public interface IDapperProxy
    {
        DynamicParameters Parameters { get; }

        IDapperProxy ClearParameters();

        IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TReturn>(Func<TFirst, TSecond, TThird, TReturn> map, string splitOn);

        IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, string splitOn);

        void Execute();

        IDapperProxy WithStoredProcedure(string storedProcedure);

        IDapperProxy AddParameter(string name, object parameter, DbType dbType, int? size = null, ParameterDirection? parameterDirection = default(ParameterDirection));
    }
}
