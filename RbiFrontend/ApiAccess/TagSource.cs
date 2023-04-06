using RbiShared.DTOs;

namespace RbiFrontend.ApiAccess;

public class TagSource : AbstractSource<TagDto>
{
    protected override string ResourceName { get; } = "tags";

    public TagSource(HttpClient http) : base(http)
    {
    }

}
