using RbiData.Entities;

namespace RbiData.Services;
public interface ITagService
{
	Task<IEnumerable<Tag>> SearchTags(string prefix);
}