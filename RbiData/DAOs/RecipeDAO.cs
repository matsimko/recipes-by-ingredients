using Dapper;
using RbiData.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RbiData.DAOs;

public class RecipeDao
{
    private readonly IManagedTransaction _mt;

    public RecipeDao(IManagedTransaction mt)
    {
        _mt = mt;
    }

    public async Task Insert(Recipe recipe)
    {
        await _mt.Connection.ExecuteAsync(
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

    public async Task AddTagToRecipe(Tag tag, long recipeId)
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

        await _mt.Connection.ExecuteAsync(
            "sp_AddTagToRecipe",
            parameters,
            _mt.Transaction,
            commandType: CommandType.StoredProcedure);

    }

    public async Task RemoveTagFromRecipe(Tag tag, long recipeId)
    {
        await _mt.Connection.ExecuteAsync(
            "sp_RemoveTagFromRecipe",
            new
            {
                TagId = tag.Id,
                recipeId,
            },
            _mt.Transaction,
            commandType: CommandType.StoredProcedure);
    }

    public async Task Update(Recipe recipe)
    {
        await _mt.Connection.ExecuteAsync(
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

    public async Task Delete(long id)
    {
        await _mt.Connection.ExecuteAsync(
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
            commandType: CommandType.StoredProcedure,
            splitOn: "UserId");
            
        return result.FirstOrDefault();
    }

    public async Task<RecipeWithTags?> GetRecipeDetail(long id)
    {
        RecipeWithTags? recipe = null;
        //as there is only Tag and Ingredient in the hierarchy,
        //I don't have to do a custom hierarchy parsing with a reader
        var result = await _mt.Connection.QueryAsync<RecipeWithTags, User?, Tag, IngredientInfo, RecipeWithTags>(
            "sp_GetRecipe",
            (r, u, t, ii) =>
            {
                if (recipe == null)
                {
                    recipe = r;
                    recipe.User = u;
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
            splitOn: "UserId, TagId");

        return recipe;
    }

    public Task<IEnumerable<RecipeWithTags>> GetRecipesForUser(long userId)
    {
        var parameters = new { userId };

        return QueryRecipesWithTags("sp_GetRecipesForUser", parameters);
    }

    public Task<IEnumerable<RecipeWithTags>> GetRecipesWhichBestMatchTags(
        IEnumerable<string> tagNames,
        long userId,
        int offset,
        int limit)
    {
        var parameters = new
        {
            TagNameList = String.Join(",", tagNames),
            userId,
            offset,
            limit
        };

        return QueryRecipesWithTags("sp_GetRecipesWhichBestMatchTags", parameters);
    }

    private async Task<IEnumerable<RecipeWithTags>> QueryRecipesWithTags(
        string sql,
        object parameters,
        CommandType commandType = CommandType.StoredProcedure)
    {
        //this implementation should be faster than the one below,
        //but for limited number of records it is irrelevant
        List<RecipeWithTags> recipes = new();
        RecipeWithTags? currentRecipe = null;
        await _mt.Connection.QueryAsync<RecipeWithTags, User?, Tag, RecipeWithTags>(
            sql,
            (r, u, t) =>
            {
                if (r.Id != currentRecipe?.Id)
                {
                    currentRecipe = r;
                    currentRecipe.User = u;
                    recipes.Add(currentRecipe);
                }
                currentRecipe.Tags.Add(t);
                return currentRecipe;
            },
            parameters,
            _mt.Transaction,
            commandType: commandType,
            splitOn: "UserId, TagId");

        return recipes;

        //var recipes = await _mt.Connection.QueryAsync<RecipeWithTags, Tag, RecipeWithTags>(
        //    sql,
        //    (r, t) =>
        //    {
        //        r.Tags.Add(t);
        //        return r;
        //    },
        //    parameters,
        //    _mt.Transaction,
        //    commandType: commandType,
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