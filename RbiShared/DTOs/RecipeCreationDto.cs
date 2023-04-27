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
	public List<InstructionDto> Instructions{ get; set; } = null!;
}