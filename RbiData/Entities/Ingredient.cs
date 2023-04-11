namespace RbiData.Entities;

public class Ingredient : Tag
{
	public int OrderNum { get; set; }
	public float? Amount { get; set; }
    public string? AmountUnit { get; set; }
}