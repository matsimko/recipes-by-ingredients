﻿using RbiData.Entities;
using RbiData.SearchObjects;

namespace RbiData.Services
{
    public interface IRecipeService
    {
        Task<Ingredient> AddIngredientToRecipe(Ingredient ingredient, long recipeId, long userId);
        Task<Tag> AddTagToRecipe(Tag tag, long recipeId, long userId);
        Task Delete(long id, long userId);
        Task<Recipe> GetRecipeDetail(long id, long? userId = null);
        Task<Recipe> Insert(Recipe recipe, long userId);
        Task RemoveIngredientFromRecipe(Ingredient ingredient, long recipeId, long userId);
        Task RemoveTagFromRecipe(Tag tag, long recipeId, long userId);
        Task<IEnumerable<Recipe>> SearchRecipes(RecipeSearch rs, long? userId = null);
        Task Update(Recipe recipe, long userId);
    }
}