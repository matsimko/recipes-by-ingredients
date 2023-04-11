using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using RbiData.Entities;
using RbiShared.SearchObjects;
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
			parameters.Add("OrderNum", ingredient.OrderNum);
			parameters.Add("Amount", ingredient.Amount);
            parameters.Add("AmountUnit", ingredient.AmountUnit);
        }

        return _mt.Connection.QuerySingleAsync<long>(
            "sp_AddTagToRecipe",
            parameters,
            _mt.Transaction,
            commandType: CommandType.StoredProcedure);

    }

	public Task AddInstructionToRecipe(Instruction instruction, long recipeId)
	{
        var parameters = new
        {
            recipeId,
            instruction.OrderNum,
            instruction.Text
        };

		return _mt.Connection.ExecuteAsync(
			"sp_AddInstructionToRecipe",
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
                tagId,
                recipeId,
            },
            _mt.Transaction,
            commandType: CommandType.StoredProcedure);
    }

	public Task RemoveInstructionFromRecipe(long orderNum, long recipeId)
	{
		return _mt.Connection.ExecuteAsync(
			"sp_RemoveInstructionFromRecipe",
			new
			{
				orderNum,
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
        var result = await QueryRecipesAsync("sp_GetRecipeDetail", new { id });
        var recipe = result.FirstOrDefault();
        var instructions = await _mt.Connection.QueryAsync<Instruction>(
			"sp_GetInstructionsForRecipe",
			new { RecipeId = id },
			_mt.Transaction,
			commandType: CommandType.StoredProcedure);
        recipe.Instructions = instructions.ToList();
        return recipe;
    }

    public Task<IEnumerable<Recipe>> SearchRecipes(RecipeSearch rs)
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

        return QueryRecipesAsync("sp_SearchRecipes", parameters);
    }

    private async Task<IEnumerable<Recipe>> QueryRecipesAsync(string storedProcedure, object parameters)
    {
        //as there is only Tag and Ingredient in the hierarchy,
        //I don't have to do a custom hierarchy parsing with a reader
        var result = await _mt.Connection.QueryAsync<Recipe, User?, Tag?, IngredientInfo?, Recipe>(
           storedProcedure,
           (r, u, t, ii) =>
           {
               r.User = u;

               if (t == null) return r;

               if (ii.IsIngredient)
               {
                   var ingredient = new Ingredient
                   {
                       Id = t.Id,
                       Name = t.Name,
                       OrderNum = ii.OrderNum,
                       Amount = ii.Amount,
                       AmountUnit = ii.AmountUnit
                   };
                   r.Ingredients.Add(ingredient);
               }
               else
               {
                   r.Tags.Add(t);
               }

               return r;
           },
           parameters,
           _mt.Transaction,
           commandType: CommandType.StoredProcedure,
           splitOn: "Id, Id, IsIngredient");

        return result.GroupBy(r => r.Id).Select(g =>
        {
            var groupedRecipe = g.First();
            groupedRecipe.Tags = g.Where(r => r.Tags.Any())
                                  .Select(r => r.Tags.First())
                                  .ToList();
            groupedRecipe.Ingredients = g.Where(r => r.Ingredients.Any())
                                         .Select(r => r.Ingredients.First())
                                         .ToList();
            return groupedRecipe;
        });
    }
}

internal class IngredientInfo
{
    public int OrderNum { get; set; }
    public bool IsIngredient { get; set; }
    public float? Amount { get; set; }
    public string? AmountUnit { get; set; }
}