namespace RbiShared.DTOs;

public class RecipeCreationDto
{
	public string Name { get; set; } = null!;
	public bool IsPublic { get; set; }
	public string? Description { get; set; }
	public int? PrepTimeMins { get; set; }
	public int? CookTimeMins { get; set; }
	public int? Servings { get; set; }
	public List<TagDto> Tags { get; set; } = null!;
	public List<IngredientDto> Ingredients { get; set; } = null!;

	public override bool Equals(object? obj)
	{
		return obj is RecipeCreationDto dto &&
			   Name == dto.Name &&
			   IsPublic == dto.IsPublic &&
			   Description == dto.Description &&
			   PrepTimeMins == dto.PrepTimeMins &&
			   CookTimeMins == dto.CookTimeMins &&
			   Servings == dto.Servings &&
			   Enumerable.SequenceEqual(Tags, dto.Tags) &&
			   Enumerable.SequenceEqual(Ingredients, dto.Ingredients);
	}
}