using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RbiData;

//implemented for SQL Server only currently
//Another way to implement this is to require a sp_QueryPaginated
//procedure implementation for the db that is used and just call it here
internal static class DbConnectionExtensions
{
    private static string CreatePaginatedSql(string sql, int offset, int limit)
    {
        //variables are int so SQL injection cannot happen
        return sql + $" OFFSET {offset} ROWS FETCH NEXT {limit} ROWS ONLY";
    }

    public static Task<IEnumerable<T>> QueryPaginated<T>(
        this IDbConnection connection,
        string sql,
        object parameters,
        int offset,
        int limit,
        IDbTransaction? transaction = null)
    {
        sql = CreatePaginatedSql(sql, offset, limit);
        return connection.QueryAsync<T>(sql, parameters, transaction);
    }

    public static Task<IEnumerable<T>> QueryPaginated<T>(
        this IDbConnection connection,
        string sql,
        Type[] types,
        Func<object[], T> map,
        object parameters,
        int offset,
        int limit,
        IDbTransaction? transaction = null)
    {
        sql = CreatePaginatedSql(sql, offset, limit);
        return connection.QueryAsync(sql, types, map, parameters, transaction);
    }
}
