using RbiShared.DTOs;

public class IngredientDto : TagDto
{
    public float? Amount { get; set; }
    public string? AmountUnit { get; set; }

    public IngredientDto()
    {
    }

    public IngredientDto(IngredientDto i) : base(i)
    {
        Amount = i.Amount;
        AmountUnit = i.AmountUnit;
    }
}