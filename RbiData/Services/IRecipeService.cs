using RbiData.Entities;

namespace RbiData.Services;
public interface IRecipeService
{
    Task AddTagToRecipe(Tag tag, long recipeId, long userId);
    Task Delete(long id, long userId);
    Task<RecipeWithTags> GetRecipeDetail(long id, long userId);
    Task<IEnumerable<RecipeWithTags>> GetRecipesForUser(long userId);
    Task<IEnumerable<RecipeWithTags>> GetRecipesWhichBestMatchTags(IEnumerable<string> tagNames, long userId, int offset = 0, int limit = 100);
    Task Insert(Recipe recipe, long userId);
    Task RemoveTagFromRecipe(Tag tag, long recipeId, long userId);
    Task Update(Recipe recipe, long userId);
}