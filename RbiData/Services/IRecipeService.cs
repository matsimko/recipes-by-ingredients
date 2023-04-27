using RbiData.Entities;
using RbiShared.SearchObjects;

namespace RbiData.Services;
public interface IRecipeService
{
	Task Delete(long id, long userId);
	Task<Recipe> GetRecipeDetail(long id, long? userId = null);
	Task<Recipe> Insert(Recipe recipe, long userId);
	Task<IEnumerable<Recipe>> SearchRecipes(RecipeSearch rs, long? userId = null);
	Task Update(Recipe recipe, long userId);
}