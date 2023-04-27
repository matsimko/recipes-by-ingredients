namespace RbiShared.DTOs;

public class RecipeDetailDto
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public bool IsPublic { get; set; }
    public UserDto? User { get; set; }
    public string? Description { get; set; }
	public int? PrepTimeMins { get; set; }
	public int? CookTimeMins { get; set; }
	public int? Servings { get; set; }
	public DateTimeOffset CreationDate { get; set; }
    public List<TagDto> Tags { get; set; } = new();
    public List<IngredientDto> Ingredients { get; set; } = new();
	public List<InstructionDto> Instructions { get; set; } = new();

    public RecipeDetailDto()
    {
    }

    public RecipeDetailDto(RecipeDetailDto r)
    {
        Id = r.Id;
        Name = r.Name;
        Description = r.Description;
        IsPublic = r.IsPublic;
        PrepTimeMins = r.PrepTimeMins;
        CookTimeMins = r.CookTimeMins;
        CreationDate = r.CreationDate;
        User = r.User;
        
        Tags = r.Tags.Select(t => new TagDto(t)).ToList();
        Ingredients = r.Ingredients.Select(i => new IngredientDto(i)).ToList();
        Instructions = r.Instructions.Select(i => new InstructionDto(i)).ToList();
	}

	public override bool Equals(object? obj)
	{
        return obj is RecipeDetailDto dto &&
               Id == dto.Id &&
               Name == dto.Name &&
               IsPublic == dto.IsPublic &&
               EqualityComparer<UserDto?>.Default.Equals(User, dto.User) &&
               Description == dto.Description &&
               PrepTimeMins == dto.PrepTimeMins &&
               CookTimeMins == dto.CookTimeMins &&
               Servings == dto.Servings &&
               CreationDate.Equals(dto.CreationDate) &&
               Enumerable.SequenceEqual(Tags, dto.Tags) &&
               Enumerable.SequenceEqual(Ingredients, dto.Ingredients) &&
               Enumerable.SequenceEqual(Instructions, dto.Instructions);
	}
}