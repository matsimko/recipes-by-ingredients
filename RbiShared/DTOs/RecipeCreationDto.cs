namespace RbiShared.DTOs;

public record RecipeCreationDto(
	string Name,
	bool IsPublic,
	string? Description,
	int? PrepTimeMins,
	int? CookTimeMins,
	int? Servings);