using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RbiData.Transactions;
public class ManagedTransactionFactory : IManagedTransactionFactory
{
    private readonly IConfiguration _configuration;

    public ManagedTransactionFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IManagedTransaction Create()
    {
        return new ManagedTransaction(_configuration.GetConnectionString("Default"));
    }
}
