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

internal class DbAccess
{
    private readonly IConfiguration _config;

    public DbAccess(IConfiguration config)
    {
        _config = config;
    }

    //Use a connection when you need to run a custom transaction
    public IDbConnection NewConnection(string connectionId = "Default")
    {
        return new SqlConnection(_config.GetConnectionString(connectionId));
    }
}
