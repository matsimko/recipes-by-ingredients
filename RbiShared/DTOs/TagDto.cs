namespace RbiShared.DTOs;

public class TagDto
{
    //public long Id { get; set; }
    public string? Name { get; set; }

    public TagDto()
    {
    }

    public TagDto(TagDto t)
    {
        //Id = t.Id;
        Name = t.Name;
    }
}
