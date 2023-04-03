using RbiShared.DTOs;

public class IngredientDto : TagDto
{
    public float? Amount { get; set; }
    public string? AmountUnit { get; set; }
}