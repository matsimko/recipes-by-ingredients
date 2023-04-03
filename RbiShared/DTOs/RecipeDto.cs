namespace RbiShared.DTOs;

public class RecipeDto
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public bool IsPublic { get; set; }
    public UserDto? User { get; set; }
    public string? Description { get; set; }
    public DateTimeOffset? CreationDate { get; set; }
    public List<TagDto>? Tags { get; set; }
    public List<IngredientDto>? Ingredients { get; set; }
}