using Dapper.Contrib.Extensions;
using Dapper;
using RbiData.DAOs;
using RbiData.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RbiData.Services;
public class RecipeService
{
    private const int DefaultRecipeLimit = 100;
    private readonly IManagedTransactionFactory _transactionFactory;

    public RecipeService(IManagedTransactionFactory transactionFactory)
    {
        _transactionFactory = transactionFactory;
    }

    public async Task Insert(Recipe recipe)
    {
        using var transaction = _transactionFactory.Create();
        var recipeDAO = new RecipeDAO(transaction);
        await recipeDAO.Insert(recipe);
    }

    public async Task AddTagToRecipe(Tag tag, Recipe recipe)
    {
        using var transaction = _transactionFactory.Create();
        var recipeDAO = new RecipeDAO(transaction);
        await recipeDAO.AddTagToRecipe(tag, recipe);
    }

    public async Task RemoveTagFromRecipe(Tag tag, Recipe recipe)
    {
        using var transaction = _transactionFactory.Create();
        var recipeDAO = new RecipeDAO(transaction);
        await recipeDAO.RemoveTagFromRecipe(tag, recipe);
    }

    public async Task Update(Recipe recipe)
    {
        using var transaction = _transactionFactory.Create();
        var recipeDAO = new RecipeDAO(transaction);
        await recipeDAO.Update(recipe);
    }

    public async Task Delete(Recipe recipe)
    {
        using var transaction = _transactionFactory.Create();
        var recipeDAO = new RecipeDAO(transaction);
        await recipeDAO.Delete(recipe);
    }

    public Task<RecipeWithTags?> GetRecipe(long id)
    {
        using var transaction = _transactionFactory.Create();
        var recipeDAO = new RecipeDAO(transaction);
        return recipeDAO.GetRecipe(id);
    }
    public Task<IEnumerable<RecipeWithTags>> GetRecipesForUser(User user)
    {
        using var transaction = _transactionFactory.Create();
        var recipeDAO = new RecipeDAO(transaction);
        return recipeDAO.GetRecipesForUser(user);
    }

    public Task<IEnumerable<RecipeWithTags>> GetRecipesWhichBestMatchTags(
        IEnumerable<string> tagNames,
        User user,
        int offset = 0,
        int limit = DefaultRecipeLimit)
    {
        using var transaction = _transactionFactory.Create();
        var recipeDAO = new RecipeDAO(transaction);
        return recipeDAO.GetRecipesWhichBestMatchTags(tagNames, user, offset, limit);
    }
}
