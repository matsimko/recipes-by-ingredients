using AutoMapper;
using RbiData.Entities;
using RbiShared.DTOs;

namespace RbiApi;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<Recipe, RecipeDto>().ReverseMap();
        CreateMap<Recipe, RecipeCreationDto>().ReverseMap();
		CreateMap<Recipe, RecipeDetailDto>().ReverseMap();

		CreateMap<Tag, TagDto>().ReverseMap();
		CreateMap<Ingredient, IngredientDto>().ReverseMap();
		CreateMap<Ingredient, IngredientCreationDto>().ReverseMap();

		CreateMap<User, UserDto>().ReverseMap();

		CreateMap<Instruction, InstructionDto>().ReverseMap();
    }
}
