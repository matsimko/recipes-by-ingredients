namespace RbiShared.DTOs;

public class RecipeDetailDto
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public bool IsPublic { get; set; }
    public UserDto? User { get; set; }
    public string? Description { get; set; }
	public int? PrepTimeMins { get; set; }
	public int? CookTimeMins { get; set; }
	public DateTimeOffset? CreationDate { get; set; }
    public List<TagDto>? Tags { get; set; }
    public List<IngredientDto>? Ingredients { get; set; }
	public List<InstructionDto>? Instructions { get; set; }
}