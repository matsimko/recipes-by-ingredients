﻿namespace RbiShared.DTOs;

public class RecipeDetailDto
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public bool IsPublic { get; set; }
    public UserDto? User { get; set; }
    public string? Description { get; set; }
	public int? PrepTimeMins { get; set; }
	public int? CookTimeMins { get; set; }
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
}