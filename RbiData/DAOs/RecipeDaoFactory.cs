using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RbiData.DAOs;
public class RecipeDaoFactory : IDaoFactory<RecipeDao>
{
    private readonly ILoggerFactory _loggerFactory;

    public RecipeDaoFactory(ILoggerFactory loggerFactory)
    {
        _loggerFactory = loggerFactory;
    }

    public RecipeDao Create(IManagedTransaction transaction)
    {
        return new RecipeDao(transaction, _loggerFactory.CreateLogger<RecipeDao>());
    }
}
