CREATE PROCEDURE [dbo].[sp_SearchRecipes]
	@name VARCHAR(200) = NULL,
	@tagNameList VARCHAR(MAX) = NULL,
	@userId INT = NULL,
	@includePrivateRecipesOfUser BIT = 1,
	@includePublicRecipes BIT = 1,
	@exactMatch BIT = 0,
	@offset INT = 0,
	@limit INT = 100
AS
BEGIN
	IF @name IS NOT NULL
	BEGIN
		exec sp_SearchRecipesByName 
			@name,
			@userId,
			@includePrivateRecipesOfUser,
			@includePublicRecipes,
			@offset,
			@limit;
		RETURN;
	END;

	IF @tagNameList IS NOT NULL
	BEGIN
		exec sp_SearchRecipesByTags
			@tagNameList,
			@userId,
			@includePrivateRecipesOfUser,
			@includePublicRecipes,
			@exactMatch,
			@offset,
			@limit;
		RETURN;
	END;

	SELECT r.Id, Name, IsPublic, CreationDate,
		UserId AS Id, Username, IsAnonymous,
		TagId AS Id, TagName AS Name, IsIngredient, Amount, AmountUnit
	FROM VI_RecipeWithTags r
	ORDER BY CreationDate
	OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY;
	RETURN;
END
