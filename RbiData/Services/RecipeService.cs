using Dapper;
using Microsoft.Extensions.Logging;
using RbiData.DAOs;
using RbiData.Entities;
using RbiData.Transactions;
using RbiShared.SearchObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace RbiData.Services;
public class RecipeService : IRecipeService
{
	private readonly ILogger<RecipeService> _logger;
    private readonly TransactionExecutor<RecipeDao> _transactionExecuter;

    public RecipeService(
		IManagedTransactionFactory transactionFactory,
		IDaoFactory<RecipeDao> recipeDaoFactory,
		ILogger<RecipeService> logger)
	{
		_logger = logger;

		_transactionExecuter = new(transactionFactory, recipeDaoFactory);
	}

	public Task<Recipe> Insert(Recipe recipe, long userId)
	{
		recipe.User = new User { Id = userId };

		return _transactionExecuter.Execute<Recipe>(async recipeDao =>
		{
			var id = await recipeDao.Insert(recipe);
			recipe = await recipeDao.GetRecipeDetail(id); //could be done in one db round-trip
			return recipe;
		});
	}

	public Task Update(Recipe recipe, long userId)
	{
		return _transactionExecuter.Execute(async recipeDao =>
		{
			await CheckIfCanModify(recipe.Id, userId, recipeDao);
			await recipeDao.Update(recipe);
		});
	}

	public Task Delete(long id, long userId)
	{
		return _transactionExecuter.Execute(async recipeDao =>
		{
            await CheckIfCanModify(id, userId, recipeDao);
            await recipeDao.Delete(id);
        });
	}

	public async Task<Recipe> GetRecipeDetail(long id, long? userId = null)
	{
		var recipe = await _transactionExecuter.Execute(recipeDao =>
		{
			return recipeDao.GetRecipeDetail(id);
		});

        if (recipe == null) throw new EntityNotFoundException("Recipe", id);
        if (!recipe.IsPublic && (recipe.User?.Id != userId || userId == null))
        {
            throw new OwnershipException();
        }
        return recipe;
	}

	public Task<IEnumerable<Recipe>> SearchRecipes(RecipeSearch rs, long? userId = null)
	{
		if (rs.UserId != null && rs.UserId != userId && rs.IncludePrivateRecipesOfUser)
		{
			throw new OwnershipException("User cannot access private recipes of other user");
		}

		return _transactionExecuter.Execute(recipeDao =>
		{
			return recipeDao.SearchRecipes(rs);
		});
	}

	private static async Task CheckIfCanModify(long recipeId, long userId, RecipeDao recipeDao)
	{
		var recipe = await recipeDao.GetRecipe(recipeId);
		if (recipe == null) throw new EntityNotFoundException("Recipe", recipeId);
		if (recipe.User?.Id != userId) throw new OwnershipException();
	}
}