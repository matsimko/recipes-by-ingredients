using RbiData.Entities;

namespace RbiApi.DTOs
{
    public class RecipeCreationDto
    {
        public string? Name { get; set; }
        public bool IsPublic { get; set; }
        public string? Description { get; set; }
    }
}
