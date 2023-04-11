namespace RbiShared.DTOs;

public class IngredientCreationDto
{
	public int OrderNum { get; set; }
	public string? Name { get; set; }
    public float? Amount { get; set; }
    public string? AmountUnit { get; set; }
}
