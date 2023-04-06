using Microsoft.AspNetCore.Mvc;
using RbiShared.DTOs;
using RbiData.Entities;
using RbiData.Services;
using AutoMapper;
using RbiShared.SearchObjects;
using System.Collections.Generic;
using System.Text.Json;

namespace RbiApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class RecipesController : ControllerBase
{
    private readonly IRecipeService _recipeService;
	private readonly IUserService _userService;
	private readonly ILogger<RecipesController> _logger;
    private readonly IMapper _mapper;

    public RecipesController(
        IRecipeService recipeService,
        IUserService userService,
        ILogger<RecipesController> logger,
        IMapper mapper)
    {
        _recipeService = recipeService;
		_userService = userService;
		_logger = logger;
        _mapper = mapper;
    }


    [HttpGet]
    public async Task<IEnumerable<RecipeDto>> Get([FromQuery] RecipeSearch? rs)
    {
        rs ??= new RecipeSearch
        {
            UserId = _userService.GetCurrentUserId(),
            IncludePrivateRecipesOfUser = true,
            IncludePublicRecipes = true
        };
        _logger.LogDebug("RecipeSearch={rs}", rs);

        var recipes = await _recipeService.SearchRecipes(rs, _userService.GetCurrentUserId());
        return _mapper.Map<IEnumerable<Recipe>, IEnumerable<RecipeDto>>(recipes);
    }

    [HttpGet("{id}")]
    public async Task<RecipeDto> Get(long id)
    {
        var recipe = await _recipeService.GetRecipeDetail(id, _userService.GetCurrentUserId());
        return _mapper.Map<RecipeDto>(recipe);
    }

    [HttpPost]
    public async Task<RecipeDto> Post(RecipeCreationDto dto)
    {
        var recipe = _mapper.Map<Recipe>(dto);
        recipe = await _recipeService.Insert(recipe, _userService.GetCurrentUserId());
        return _mapper.Map<RecipeDto>(recipe);
    }

    [HttpPost("{id}/Tags")]
    public async Task<TagDto> AddTagToRecipe(long id, [FromBody] string name)
    {
        var tag = await _recipeService.AddTagToRecipe(new Tag { Name = name }, id, _userService.GetCurrentUserId());
        return _mapper.Map<TagDto>(tag);
    }

    [HttpPost("{id}/Ingredients")]
    public async Task<IngredientDto> AddIngredientToRecipe(long id, IngredientCreationDto dto)
    {
        var ingredient = _mapper.Map<Ingredient>(dto);
        ingredient = await _recipeService.AddIngredientToRecipe(ingredient, id, _userService.GetCurrentUserId());
        return _mapper.Map<IngredientDto>(ingredient);
    }

    [HttpPut("{id}")]
    public Task Put(int id, RecipeCreationDto dto)
    {
        var recipe = _mapper.Map<Recipe>(dto);
        recipe.Id = id;
        return _recipeService.Update(recipe, _userService.GetCurrentUserId());
    }

    [HttpDelete("{id}")]
    public Task Delete(int id)
    {
        return _recipeService.Delete(id, _userService.GetCurrentUserId());
    }

    [HttpDelete("{id}/Tags/{tagId}")]
    public Task RemoveTagFromRecipe(long id, long tagId)
    {
        return _recipeService.RemoveTagFromRecipe(tagId, id, _userService.GetCurrentUserId());
    }

    [HttpDelete("{id}/Ingredients/{ingredientId}")]
    public Task RemoveIngredientsFromRecipe(long id, long ingredientId)
    {
        return _recipeService.RemoveIngredientFromRecipe(ingredientId, id, _userService.GetCurrentUserId());
    }

}
