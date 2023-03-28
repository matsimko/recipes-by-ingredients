using Dapper;
using Microsoft.Data.SqlClient;
//using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace RbiData;

/// <summary>
/// This class is designed to allow a single transaction within a single connection.
/// </summary>
public class ManagedTransaction : IManagedTransaction
{
    private IDbConnection? _connection;
    private IDbTransaction? _transaction;

    public ManagedTransaction(string connectionString)
    {
        _connection = new SqlConnection(connectionString);
    }

    public IDbConnection Connection
    {
        get
        {
            if (_connection == null)
            {
                throw new InvalidOperationException("This instance has already completed a transaction. Create a new one.");
            }
            return _connection;
        }
    }

    public IDbTransaction Transaction
    {
        get
        {
            if (_transaction == null)
            {
                Connection.Open();
                _transaction = Connection.BeginTransaction();
            }
            return _transaction;
        }
    }

    public void Commit()
    {
        if (_transaction == null) throw new InvalidOperationException("Transaction hasn't started yet");
        _transaction.Commit();
        _transaction = null;
        Dispose();
    }

    public void Dispose()
    {
        if (_transaction != null)
        {
            _transaction.Rollback();
            _transaction.Dispose();
            _transaction = null;
        }
        if (_connection != null)
        {
            _connection.Dispose();
            _connection = null;
        }
    }
}
