namespace RbiData.Entities;

public class UsedIngredient
{
    public Ingredient? Ingredient { get; set; }
    public float? Amount { get; set; }
    public string? AmountUnit { get; set; }
}