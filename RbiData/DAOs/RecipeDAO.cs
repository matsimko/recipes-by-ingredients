﻿using Dapper;
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

    public Task<IEnumerable<RecipeWithTags>> GetRecipesForUser(User user)
    {
        return _mt.Connection.QueryAsync<RecipeWithTags>(
            "sp_GetRecipesForUser",
            new { UserId = user.Id },
            _mt.Transaction,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<RecipeWithTags?> GetRecipe(long id)
    {
        RecipeWithTags? recipe = null;
        var result = await _mt.Connection.QueryAsync<RecipeWithTags, User, Tag, RecipeWithTags>(
            "sp_GetRecipe",
            (r, u, t) =>
            {
                if(recipe == null)
                {
                    recipe = r;
                    recipe.User = u;
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

    public async Task<IEnumerable<RecipeWithTags>> GetRecipesWhichBestMatchTags(
        IEnumerable<string> tagNames,
        User user,
        int offset = 0,
        int limit = DefaultRecipeLimit)
    {
        var recipes = await _mt.Connection.QueryAsync<RecipeWithTags, Tag, RecipeWithTags>(
            "sp_GetRecipesWhichBestMatchTags",
            (r, t) =>
            {
                r.Tags.Add(t);
                return r;
            },
            new
            {
                TagNameList = String.Join(",", tagNames),
                UserId = user.Id,
                offset,
                limit
            },
            _mt.Transaction,
            commandType: CommandType.StoredProcedure,
            splitOn: "TagId");

        return recipes.GroupBy(r => r.Id).Select(g =>
        {
            var groupedRecipe = g.First();
            groupedRecipe.Tags = g.Select(r => r.Tags.First()).ToList();
            return groupedRecipe;
        });
    }
}