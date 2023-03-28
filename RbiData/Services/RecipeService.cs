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

    public RecipeService(IManagedTransactionFactory transactionFactory)
    {
        _transactionFactory = transactionFactory;
    }

    public async Task Insert(Recipe recipe, long userId)
    {
        recipe.User = new User { Id = userId };

        using var transaction = _transactionFactory.Create();
        var recipeDAO = new RecipeDAO(transaction);
        await recipeDAO.Insert(recipe);
        transaction.Commit();
    }

    public async Task AddTagToRecipe(Tag tag, long recipeId, long userId)
    {
        using var transaction = _transactionFactory.Create();
        var recipeDAO = new RecipeDAO(transaction);
        await ValidateExistenceAndOwnershipAsync(recipeDAO, recipeId, userId);
        await recipeDAO.AddTagToRecipe(tag, recipeId);
        transaction.Commit();
    }

    public async Task RemoveTagFromRecipe(Tag tag, long recipeId, long userId)
    {
        using var transaction = _transactionFactory.Create();
        var recipeDAO = new RecipeDAO(transaction);
        await ValidateExistenceAndOwnershipAsync(recipeDAO, recipeId, userId);
        await recipeDAO.RemoveTagFromRecipe(tag, recipeId);
        transaction.Commit();
    }

    public async Task Update(Recipe recipe, long userId)
    {
        using var transaction = _transactionFactory.Create();
        var recipeDAO = new RecipeDAO(transaction);
        await ValidateExistenceAndOwnershipAsync(recipeDAO, recipe.Id, userId);
        await recipeDAO.Update(recipe);
        transaction.Commit();
    }

    public async Task Delete(long id, long userId)
    {
        using var transaction = _transactionFactory.Create();
        var recipeDAO = new RecipeDAO(transaction);
        await ValidateExistenceAndOwnershipAsync(recipeDAO, id, userId);
        await recipeDAO.Delete(id);
        transaction.Commit();
    }

    public async Task<RecipeWithTags> GetRecipeDetail(long id, long userId)
    {
        using var transaction = _transactionFactory.Create();
        var recipeDAO = new RecipeDAO(transaction);
        var recipe = await recipeDAO.GetRecipeDetail(id);
        transaction.Commit();
        ValidateExistenceAndOwnership(recipe, userId);
        return recipe;
    }

    public Task<IEnumerable<RecipeWithTags>> GetRecipesForUser(long userId)
    {
        using var transaction = _transactionFactory.Create();
        var recipeDAO = new RecipeDAO(transaction);
        var recipes = recipeDAO.GetRecipesForUser(userId);
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
        var recipeDAO = new RecipeDAO(transaction);
        var recipes = recipeDAO.GetRecipesWhichBestMatchTags(tagNames, userId, offset, limit);
        transaction.Commit();
        return recipes;
    }

    private static async Task ValidateExistenceAndOwnershipAsync(RecipeDAO recipeDAO, long recipeId, long? userId)
    {
        var recipe = await recipeDAO.GetRecipe(recipeId);
        ValidateExistenceAndOwnership(recipe, userId);
    }

    private static void ValidateExistenceAndOwnership(Recipe? recipe, long? userId)
    {
        if (recipe == null) throw new EntityNotFoundException("Recipe does not exist");
        if (recipe.User?.Id != userId) throw new OwnershipException("Recipe is not owned by user");
    }
}
