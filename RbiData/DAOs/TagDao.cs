using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using RbiData.Entities;
using RbiData.Transactions;
using RbiShared.SearchObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RbiData.DAOs;

public class TagDao
{
    private readonly IManagedTransaction _mt;
    private readonly ILogger<TagDao> _logger;

    public TagDao(IManagedTransaction mt, ILogger<TagDao> logger)
    {
        _mt = mt;
        _logger = logger;
    }

    public Task<IEnumerable<Tag>> SearchTags(string prefix)
    {
        return _mt.Connection.QueryAsync<Tag>(
            "sp_SearchTags",
            new { prefix },
            _mt.Transaction,
            commandType: CommandType.StoredProcedure);
    }
}