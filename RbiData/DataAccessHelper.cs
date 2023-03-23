using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace RbiData;

/// <summary>
/// This class allows creation of a new connection and 
/// a set of convenience methods for executing a single command in a new connection,
/// assuming auto-commit is on.
/// (otherwise the class would have to be reimplemented to begin and commit a transaction in each method)
/// </summary>
/// <typeparam name="T"></typeparam>
internal class DataAccessHelper<T> where T : class
{
    private IConfiguration _config;

    public DataAccessHelper(IConfiguration config)
    {
        _config = config;
    }

    //Use a connection when you need to run a custom transaction
    public IDbConnection NewConnection(string connectionId = "Default")
    {
        return new SqlConnection(_config.GetConnectionString(connectionId));
    }

    public Task<IEnumerable<T>> GetAll()
    {
        using var connection = NewConnection();
        return connection.GetAllAsync<T>();
    }

    public Task<T> Get(int id)
    {
        using var connection = NewConnection();
        return connection.GetAsync<T>(id);
    }

    public async Task Insert(T entity)
    {
        using var connection = NewConnection();
        await connection.InsertAsync(entity);
    }

    public async Task Update(T entity)
    {
        using var connection = NewConnection();
        await connection.UpdateAsync(entity);
    }

    public async Task Delete(T entity)
    {
        using var connection = NewConnection();
        await connection.DeleteAsync(entity);
    }

    public Task<IEnumerable<T>> Query(string sql, object? parameters = null)
    {
        using var connection = NewConnection();
        return connection.QueryAsync<T>(sql, parameters);
    }

    public async Task<T?> QuerySingle(string sql, object parameters)
    {
        using var connection = NewConnection();
        var results = await connection.QueryAsync<T>(sql, parameters);
        return results.SingleOrDefault();
    }

    public Task<IEnumerable<T>> QueryStoredProcedure(string storedProcedure, object parameters)
    {
        using var connection = NewConnection();
        return connection.QueryAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
    }

    public async Task<T?> QueryStoredProcedureSingle(string storedProcedure, object parameters)
    {
        var results = await QueryStoredProcedure(storedProcedure, parameters);
        return results.SingleOrDefault();
    }

    public async Task ExecuteStoredProcedure(string storedProcedure, object parameters)
    {
        using var connection = NewConnection();
        await connection.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
    }



}
