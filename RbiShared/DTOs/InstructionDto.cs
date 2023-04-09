namespace RbiShared.DTOs;

public class InstructionDto
{
	public int OrderNum { get; set; }
	public string? Text { get; set; }

	public InstructionDto()
	{
	}

    public InstructionDto(InstructionDto i)
    {
		OrderNum = i.OrderNum;
		Text = i.Text;
    }
}