using RbiShared.DTOs;

namespace RbiFrontend.ApiAccess;

public class RecipeSource : AbstractSource<RecipeDto, RecipeCreationDto, RecipeDetailDto>
{
    public RecipeSource(HttpClient http) : base(http)
    {
    }

    protected override string ResourceName { get; } = "recipes";
}
