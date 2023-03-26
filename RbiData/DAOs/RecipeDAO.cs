using Dapper;
using Dapper.Contrib.Extensions;
using RbiData.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RbiData.DAOs;

//A query for recipes can be made based on ingredients, or tags, but
//not both because a recipe with N ingredients and M tags
//would require N * M rows if it were to be retrieved in a single query
public class RecipeDAO
{
    private const int DefaultRecipeLimit = 100;
    private readonly IManagedTransaction _mt;

    public RecipeDAO(IManagedTransaction mt)
    {
        _mt = mt;
    }

    public async Task Insert(Recipe recipe)
    {
        await _mt.Connection.InsertAsync(recipe, _mt.Transaction);
    }

    public async Task Update(Recipe recipe)
    {
        await _mt.Connection.UpdateAsync(recipe, _mt.Transaction);
    }

    public async Task Delete(Recipe recipe)
    {
        await _mt.Connection.DeleteAsync(recipe, _mt.Transaction);
    }

    public Task<IEnumerable<RecipeWithIngredients>> GetRecipesForUser(User user)
    {
        return _mt.Connection.QueryAsync<RecipeWithIngredients>(
            "sp_GetRecipesForUser",
            new { UserId = user.Id },
            _mt.Transaction,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<RecipeDetail> GetRecipe(long id)
    {
        var recipe = await _mt.Connection.GetAsync<RecipeDetail>(id, _mt.Transaction);

        recipe.Ingredients = await _mt.Connection.QueryAsync<Ingredient, UsedIngredient, UsedIngredient>(
            "sp_GetIngredientsForRecipe",
            (ingredient, usedIngredient) =>
            {
                usedIngredient.Ingredient = ingredient;
                return usedIngredient;
            },
            new { RecipeId = id },
            _mt.Transaction,
            commandType: CommandType.StoredProcedure,
            splitOn: "Amount");

        recipe.Tags = await _mt.Connection.QueryAsync<Tag>(
            "sp_GetTagsForRecipe",
            new { RecipeId = id },
            _mt.Transaction,
            commandType: CommandType.StoredProcedure);

        return recipe;

    }

    // I might reimplement the GetRecipesWhichBestMatch SQL to prevent code duplication
    // by generating dynamic SQL either here or in a stored procedure.

    public Task<IEnumerable<RecipeWithIngredients>> GetRecipesWhichBestMatchIngredients(
        IEnumerable<Ingredient> ingredients,
        User user,
        int offset = 0,
        int limit = DefaultRecipeLimit)
    {
        return GetRecipesWhichBestMatch<RecipeWithIngredients, Ingredient>(ingredients, user, offset, limit);
    }

    public Task<IEnumerable<RecipeWithTags>> GetRecipesWhichBestMatchTags(
        IEnumerable<Tag> tags,
        User user,
        int offset = 0,
        int limit = DefaultRecipeLimit)
    {
        return GetRecipesWhichBestMatch<RecipeWithTags, Tag>(tags, user, offset, limit);
    }

    //This overengineering to prevent duplication of two methods may obviously cost some performance
    private async Task<IEnumerable<T>> GetRecipesWhichBestMatch<T, U>(
        IEnumerable<U> entities,
        User user,
        int offset,
        int limit) where T : Recipe
    {
        string entityName = typeof(U).Name;
        var entitiesProp = typeof(T).GetProperty($"{entityName}s");
        var getEntities = (T r) => (IEnumerable<U>)entitiesProp.GetValue(r);
        var setEntities = (T r, IEnumerable<U> value) => entitiesProp.SetValue(r, value);

        var recipes = await _mt.Connection.QueryAsync<T, U, T>(
            $"sp_GetRecipesWhichBestMatch{entityName}s",
            (recipe, entity) =>
            {
                getEntities(recipe).Append(entity);
                return recipe;
            },
            new DynamicParameters(new Dictionary<string, object>
            {
                {$"{entityName}IdList", String.Join(",", entities) },
                {"UserId ", user.Id },
                {"Offset", offset },
                { "Limit", limit }
            }),
            _mt.Transaction,
            commandType: CommandType.StoredProcedure,
            splitOn: $"{entityName}Id");

        return recipes.GroupBy(r => r.Id).Select(g =>
        {
            var groupedRecipe = g.First();
            setEntities(groupedRecipe, g.Select(r => getEntities(r).First()));
            return groupedRecipe;
        });
    }
}