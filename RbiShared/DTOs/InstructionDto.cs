namespace RbiShared.DTOs;

public class InstructionDto
{
	public string? Text { get; set; }

	public InstructionDto()
	{
	}

    public InstructionDto(InstructionDto i)
    {
		Text = i.Text;
    }
}