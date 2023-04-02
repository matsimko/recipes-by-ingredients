using Microsoft.AspNetCore.Mvc;
using RbiApi.DTOs;
using RbiData.Entities;
using RbiData.SearchObjects;
using RbiData.Services;

namespace RbiApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class RecipesController : ControllerBase
{
    private readonly IRecipeService _recipeService;
    private readonly ILogger<RecipesController> _logger;

    public RecipesController(IRecipeService recipeService, ILogger<RecipesController> logger)
    {
        _recipeService = recipeService;
        _logger = logger;
    }


    [HttpGet]
    public Task<IEnumerable<Recipe>> Get([FromQuery] RecipeSearch? rs)
    {
        rs ??= new RecipeSearch
        {
            UserId = GetCurrentUserId(),
            IncludePrivateRecipesOfUser = true,
            IncludePublicRecipes = true
        };
        _logger.LogDebug("RecipeSearch={rs}", rs);
        return _recipeService.SearchRecipes(rs, GetCurrentUserId());
    }

    [HttpGet("{id}")]
    public Task<Recipe> Get(long id)
    {
        return _recipeService.GetRecipeDetail(id, GetCurrentUserId());
    }

    [HttpPost]
    public Task<Recipe> Post(RecipeCreationDto dto)
    {
        var recipe = MapDto(dto);
        return _recipeService.Insert(recipe, GetCurrentUserId());
    }

    [HttpPost("{id}/Tags")]
    public Task<Tag> AddTagToRecipe(long id, [FromBody] string name)
    {
        return _recipeService.AddTagToRecipe(new Tag { Name = name }, id, GetCurrentUserId());
    }

    [HttpPost("{id}/Ingredients")]
    public Task<Ingredient> AddIngredientToRecipe(long id, IngredientCreationDto dto)
    {
        var ingredient = new Ingredient
        {
            Name = dto.Name,
            Amount = dto.Amount,
            AmountUnit = dto.AmountUnit
        };
        return _recipeService.AddIngredientToRecipe(ingredient, id, GetCurrentUserId());
    }

    [HttpPut("{id}")]
    public Task Put(int id, RecipeCreationDto dto)
    {
        var recipe = MapDto(dto);
        recipe.Id = id;
        return _recipeService.Update(recipe, GetCurrentUserId());
    }

    [HttpDelete("{id}")]
    public Task DeleteAsync(int id)
    {
        return _recipeService.Delete(id, GetCurrentUserId());
    }

    [HttpDelete("{id}/Tags/{tagId}")]
    public Task RemoveTagFromRecipe(long id, long tagId)
    {
        return _recipeService.RemoveTagFromRecipe(new Tag { Id = tagId }, id, GetCurrentUserId());
    }

    [HttpDelete("{id}/Ingredients/{ingredientId}")]
    public Task RemoveIngredientsFromRecipe(long id, long ingredientId)
    {
        return _recipeService.RemoveIngredientFromRecipe(new Ingredient { Id = ingredientId }, id, GetCurrentUserId());
    }

    private static Recipe MapDto(RecipeCreationDto dto)
    {
        return new Recipe
        {
            Name = dto.Name,
            Description = dto.Description,
            IsPublic = dto.IsPublic
        };
    }


    private long GetCurrentUserId()
    {
        return 1;
    }


}
