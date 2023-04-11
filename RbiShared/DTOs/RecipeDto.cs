namespace RbiShared.DTOs;

public class RecipeDto
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public bool IsPublic { get; set; }
    public UserDto? User { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public List<string> Tags { get; set; } = null!;
    public List<string> Ingredients { get; set; } = null!;
}