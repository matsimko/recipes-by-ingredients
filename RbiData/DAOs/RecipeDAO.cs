using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using RbiData.Entities;
using RbiData.SearchObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RbiData.DAOs;

public class RecipeDao
{
    private readonly IManagedTransaction _mt;
    private readonly ILogger<RecipeDao> _logger;

    public RecipeDao(IManagedTransaction mt, ILogger<RecipeDao> logger)
    {
        _mt = mt;
        _logger = logger;
    }

    public Task<long> Insert(Recipe recipe)
    {
        return _mt.Connection.QuerySingleAsync<long>(
            "sp_InsertRecipe",
            new
            {
                recipe.Name,
                recipe.Description,
                recipe.IsPublic,
                UserId = recipe.User.Id
            },
            _mt.Transaction,
            commandType: CommandType.StoredProcedure);
    }

    public Task<long> AddTagToRecipe(Tag tag, long recipeId)
    {
        var parameters = new DynamicParameters(new
        {
            Name = tag.Name,
            recipeId
        });
        if (tag is Ingredient ingredient)
        {
            parameters.Add("IsIngredient", true);
            parameters.Add("Amount", ingredient.Amount);
            parameters.Add("AmountUnit", ingredient.AmountUnit);
        }

        return _mt.Connection.QuerySingleAsync<long>(
            "sp_AddTagToRecipe",
            parameters,
            _mt.Transaction,
            commandType: CommandType.StoredProcedure);

    }

    public Task RemoveTagFromRecipe(long tagId, long recipeId)
    {
        return _mt.Connection.ExecuteAsync(
            "sp_RemoveTagFromRecipe",
            new
            {
                TagId = tagId,
                recipeId,
            },
            _mt.Transaction,
            commandType: CommandType.StoredProcedure);
    }

    public Task Update(Recipe recipe)
    {
        return _mt.Connection.ExecuteAsync(
           "sp_UpdateRecipe",
           new
           {
               recipe.Id,
               recipe.Name,
               recipe.Description,
               recipe.IsPublic,
           },
           _mt.Transaction,
           commandType: CommandType.StoredProcedure);
    }

    public Task Delete(long id)
    {
        return _mt.Connection.ExecuteAsync(
            "sp_DeleteRecipe",
            new { id },
            _mt.Transaction,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<Recipe?> GetRecipe(long id)
    {
        var result = await _mt.Connection.QueryAsync<Recipe, User?, Recipe>(
            "sp_GetRecipe",
            (r, u) =>
            {
                r.User = u;
                return r;
            },
            new { id },
            _mt.Transaction,
            commandType: CommandType.StoredProcedure);
            
        return result.FirstOrDefault();
    }

    public async Task<Recipe?> GetRecipeDetail(long id)
    {
        Recipe? recipe = null;
        //as there is only Tag and Ingredient in the hierarchy,
        //I don't have to do a custom hierarchy parsing with a reader
        var result = await _mt.Connection.QueryAsync<Recipe, User?, Tag?, IngredientInfo?, Recipe>(
            "sp_GetRecipeDetail",
            (r, u, t, ii) =>
            {
                if (recipe == null)
                {
                    recipe = r;
                    recipe.User = u;
                    recipe.Tags = new();
                }

                if (t == null)
                {
                    return recipe;
                }

                if (ii.IsIngredient)
                {
                    t = new Ingredient
                    {
                        Id = t.Id,
                        Name = t.Name,
                        Amount = ii.Amount,
                        AmountUnit = ii.AmountUnit
                    };
                }

                recipe.Tags.Add(t);
                return recipe;
            },
            new { id },
            _mt.Transaction,
            commandType: CommandType.StoredProcedure,
            splitOn: "Id, Id, IsIngredient");

        return recipe;
    }

    public async Task<IEnumerable<Recipe>> SearchRecipes(RecipeSearch rs)
    {
        var parameters = new
        {
            rs.Name,
            TagNameList = !rs.TagNames.IsNullOrEmpty() ? string.Join(",", rs.TagNames) : null,
            rs.UserId,
            rs.IncludePrivateRecipesOfUser,
            rs.IncludePublicRecipes,
            rs.ExactMatch,
            rs.Offset,
            rs.Limit
        };

        _logger.LogDebug("SearchRecipe parameters={p}", JsonSerializer.Serialize(parameters));

        //this implementation should be faster than the one below,
        //but for limited number of records it is irrelevant
        List<Recipe> recipes = new();
        Recipe? currentRecipe = null;
        await _mt.Connection.QueryAsync<Recipe, User?, Tag?, Recipe>(
            "sp_SearchRecipes",
            (r, u, t) =>
            {
                if (r.Id != currentRecipe?.Id)
                {
                    currentRecipe = r;
                    currentRecipe.User = u;
                    currentRecipe.Tags = new();
                    recipes.Add(currentRecipe);
                }

                if (t != null)
                {
                    currentRecipe.Tags.Add(t);
                }

                return currentRecipe;
            },
            parameters,
            _mt.Transaction,
            commandType: CommandType.StoredProcedure);
        _logger.LogDebug("Recipes count={c}", recipes.Count);
        return recipes;

        //var recipes = await _mt.Connection.QueryAsync<RecipeWithTags, Tag, RecipeWithTags>(
        //    "sp_SearchRecipes",
        //    (r, t) =>
        //    {
        //        r.Tags.Add(t);
        //        return r;
        //    },
        //    parameters,
        //    _mt.Transaction,
        //    commandType: CommandType.StoredProcedure,
        //    splitOn: "TagId");

        //return recipes.GroupBy(r => r.Id).Select(g =>
        //{
        //    var groupedRecipe = g.First();
        //    groupedRecipe.Tags = g.Select(r => r.Tags.First()).ToList();
        //    return groupedRecipe;
        //});
    }
}

internal class IngredientInfo
{
    public bool IsIngredient { get; set; }
    public float? Amount { get; set; }
    public string? AmountUnit { get; set; }
}