using Microsoft.Extensions.Logging;
using RbiData.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RbiData.DAOs;
public class TagDaoFactory : IDaoFactory<TagDao>
{
    private readonly ILoggerFactory _loggerFactory;

    public TagDaoFactory(ILoggerFactory loggerFactory)
    {
        _loggerFactory = loggerFactory;
    }

    public TagDao Create(IManagedTransaction transaction)
    {
        return new TagDao(transaction, _loggerFactory.CreateLogger<TagDao>());
    }
}
