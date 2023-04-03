namespace RbiShared.DTOs;

public class UserDto
{
    public long Id { get; set; }

    public string? Username { get; set; }

    //If a user is anonymous, their public recipes won't display their username to others
    public string? IsAnonymous { get; set; }
}