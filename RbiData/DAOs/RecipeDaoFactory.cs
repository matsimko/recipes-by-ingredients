using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RbiData.DAOs;
public class RecipeDaoFactory : IDaoFactory<RecipeDao>
{
    public RecipeDao Create(IManagedTransaction transaction)
    {
        return new RecipeDao(transaction);
    }
}
