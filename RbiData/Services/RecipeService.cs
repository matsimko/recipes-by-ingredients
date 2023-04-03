using Dapper;
using Microsoft.Extensions.Logging;
using RbiData.DAOs;
using RbiData.Entities;
using RbiData.SearchObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RbiData.Services;
public partial class RecipeService : IRecipeService
{
    private readonly IManagedTransactionFactory _transactionFactory;
    private readonly IDaoFactory<RecipeDao> _recipeDaoFactory;
    private readonly ILogger<RecipeService> _logger;

    public RecipeService(
        IManagedTransactionFactory transactionFactory,
        IDaoFactory<RecipeDao> recipeDaoFactory,
        ILogger<RecipeService> logger)
    {
        _transactionFactory = transactionFactory;
        _recipeDaoFactory = recipeDaoFactory;
        _logger = logger;
    }

    public async Task<Recipe> Insert(Recipe recipe, long userId)
    {
        recipe.User = new User { Id = userId };

        using var transaction = _transactionFactory.Create();
        var recipeDao = _recipeDaoFactory.Create(transaction);

        var id = await recipeDao.Insert(recipe);
        recipe = await recipeDao.GetRecipeDetail(id); //could be done in one db round-trip
        transaction.Commit();
        return recipe;
    }

    public async Task<Tag> AddTagToRecipe(Tag tag, long recipeId, long userId)
    {
        using var transaction = _transactionFactory.Create();
        var recipeDao = _recipeDaoFactory.Create(transaction);
        await CheckIfCanModify(recipeId, userId, recipeDao);
        tag.Id = await recipeDao.AddTagToRecipe(tag, recipeId);
        transaction.Commit();
        return tag;
    }

    public async Task<Ingredient> AddIngredientToRecipe(Ingredient ingredient, long recipeId, long userId)
    {
        //do ingredient-specific validation here

        return (Ingredient)await AddTagToRecipe(ingredient, recipeId, userId);
    }

    public async Task RemoveTagFromRecipe(long tagId, long recipeId, long userId)
    {
        using var transaction = _transactionFactory.Create();
        var recipeDao = _recipeDaoFactory.Create(transaction);
        await CheckIfCanModify(recipeId, userId, recipeDao);
        await recipeDao.RemoveTagFromRecipe(tagId, recipeId);
        transaction.Commit();
    }

    public Task RemoveIngredientFromRecipe(long ingredientId, long recipeId, long userId)
    {
        return RemoveTagFromRecipe(ingredientId, recipeId, userId);
    }
    public async Task Update(Recipe recipe, long userId)
    {
        using var transaction = _transactionFactory.Create();
        var recipeDao = _recipeDaoFactory.Create(transaction);
        await CheckIfCanModify(recipe.Id, userId, recipeDao);
        await recipeDao.Update(recipe);
        transaction.Commit();
    }

    public async Task Delete(long id, long userId)
    {
        using var transaction = _transactionFactory.Create();
        var recipeDao = _recipeDaoFactory.Create(transaction);
        await CheckIfCanModify(id, userId, recipeDao);
        await recipeDao.Delete(id);
        transaction.Commit();
    }

    public async Task<Recipe> GetRecipeDetail(long id, long? userId = null)
    {
        using var transaction = _transactionFactory.Create();
        var recipeDao = _recipeDaoFactory.Create(transaction);
        var recipe = await recipeDao.GetRecipeDetail(id);
        transaction.Commit();
        if (recipe == null) throw new EntityNotFoundException();
        if (!recipe.IsPublic && (recipe.User?.Id != userId || userId == null))
        {
            throw new OwnershipException();
        }

        return recipe;
    }

    public async Task<IEnumerable<Recipe>> SearchRecipes(RecipeSearch rs, long? userId = null)
    {
        if (rs.UserId != null && rs.UserId != userId && rs.IncludePrivateRecipesOfUser)
        {
            throw new OwnershipException("User cannot access private recipes of other user");
        }

        using var transaction = _transactionFactory.Create();
        var recipeDao = _recipeDaoFactory.Create(transaction);
        var recipes = await recipeDao.SearchRecipes(rs);
        transaction.Commit();
        return recipes;
    }

    private static async Task CheckIfCanModify(long recipeId, long userId, RecipeDao recipeDao)
    {
        var recipe = await recipeDao.GetRecipe(recipeId);
        if (recipe == null) throw new EntityNotFoundException();
        if (recipe.User?.Id != userId) throw new OwnershipException();
    }
}