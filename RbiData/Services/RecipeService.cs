using Dapper;
using RbiData.DAOs;
using RbiData.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RbiData.Services;
public class RecipeService : IRecipeService
{
    private const int DefaultRecipeLimit = 100;
    private readonly IManagedTransactionFactory _transactionFactory;
    private readonly IDaoFactory<RecipeDao> _recipeDaoFactory;

    public RecipeService(IManagedTransactionFactory transactionFactory, IDaoFactory<RecipeDao> recipeDaoFactory)
    {
        _transactionFactory = transactionFactory;
        _recipeDaoFactory = recipeDaoFactory;
    }

    public async Task Insert(Recipe recipe, long userId)
    {
        recipe.User = new User { Id = userId };

        using var transaction = _transactionFactory.Create();
        var recipeDao = _recipeDaoFactory.Create(transaction);
        await recipeDao.Insert(recipe);
        transaction.Commit();
    }

    public async Task AddTagToRecipe(Tag tag, long recipeId, long userId)
    {
        using var transaction = _transactionFactory.Create();
        var recipeDao = _recipeDaoFactory.Create(transaction);
        await ValidateExistenceAndOwnershipAsync(recipeDao, recipeId, userId);
        await recipeDao.AddTagToRecipe(tag, recipeId);
        transaction.Commit();
    }

    public async Task RemoveTagFromRecipe(Tag tag, long recipeId, long userId)
    {
        using var transaction = _transactionFactory.Create();
        var recipeDao = _recipeDaoFactory.Create(transaction);
        await ValidateExistenceAndOwnershipAsync(recipeDao, recipeId, userId);
        await recipeDao.RemoveTagFromRecipe(tag, recipeId);
        transaction.Commit();
    }

    public async Task Update(Recipe recipe, long userId)
    {
        using var transaction = _transactionFactory.Create();
        var recipeDao = _recipeDaoFactory.Create(transaction);
        await ValidateExistenceAndOwnershipAsync(recipeDao, recipe.Id, userId);
        await recipeDao.Update(recipe);
        transaction.Commit();
    }

    public async Task Delete(long id, long userId)
    {
        using var transaction = _transactionFactory.Create();
        var recipeDao = _recipeDaoFactory.Create(transaction);
        await ValidateExistenceAndOwnershipAsync(recipeDao, id, userId);
        await recipeDao.Delete(id);
        transaction.Commit();
    }

    public async Task<RecipeWithTags> GetRecipeDetail(long id, long userId)
    {
        using var transaction = _transactionFactory.Create();
        var recipeDao = _recipeDaoFactory.Create(transaction);
        var recipe = await recipeDao.GetRecipeDetail(id);
        transaction.Commit();
        ValidateExistenceAndOwnership(recipe, userId);
        return recipe;
    }

    public Task<IEnumerable<RecipeWithTags>> GetRecipesForUser(long userId)
    {
        using var transaction = _transactionFactory.Create();
        var recipeDao = _recipeDaoFactory.Create(transaction);
        var recipes = recipeDao.GetRecipesForUser(userId);
        transaction.Commit();
        return recipes;
    }

    public Task<IEnumerable<RecipeWithTags>> GetRecipesWhichBestMatchTags(
        IEnumerable<string> tagNames,
        long userId,
        int offset = 0,
        int limit = DefaultRecipeLimit)
    {
        using var transaction = _transactionFactory.Create();
        var recipeDao = _recipeDaoFactory.Create(transaction);
        var recipes = recipeDao.GetRecipesWhichBestMatchTags(tagNames, userId, offset, limit);
        transaction.Commit();
        return recipes;
    }

    private static async Task ValidateExistenceAndOwnershipAsync(RecipeDao recipeDao, long recipeId, long? userId)
    {
        var recipe = await recipeDao.GetRecipe(recipeId);
        ValidateExistenceAndOwnership(recipe, userId);
    }

    private static void ValidateExistenceAndOwnership(Recipe? recipe, long? userId)
    {
        if (recipe == null) throw new EntityNotFoundException("Recipe does not exist");
        if (recipe.User?.Id != userId) throw new OwnershipException("Recipe is not owned by user");
    }
}
