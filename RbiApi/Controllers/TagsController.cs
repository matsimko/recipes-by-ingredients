using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using RbiData.Entities;
using RbiData.Services;
using RbiShared.DTOs;
using RbiShared.SearchObjects;

namespace RbiApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TagsController : ControllerBase
{
	private readonly ITagService _tagService;
	private readonly IMapper _mapper;

	public TagsController(ITagService tagService, IMapper mapper)
	{
		_tagService = tagService;
		_mapper = mapper;
	}

	[HttpGet]
	public async Task<IEnumerable<TagDto>> Get([FromQuery, BindRequired] string prefix)
	{
		var tags = await _tagService.SearchTags(prefix);
		return _mapper.Map<IEnumerable<Tag>, IEnumerable<TagDto>>(tags);
	}
}
