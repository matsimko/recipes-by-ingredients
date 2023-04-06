using System.Text.Json;

namespace RbiShared.SearchObjects;

public class RecipeSearch
{
    public string? Name { get; set; }
    public List<string>? TagNames { get; set; }
    public long? UserId { get; set; }
    public bool IncludePrivateRecipesOfUser { get; set; }
    public bool IncludePublicRecipes { get; set; }
    public bool ExactMatch { get; set; }
    public int Offset { get; set; } = 0;
    public int Limit { get; set; } = 100;

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}
