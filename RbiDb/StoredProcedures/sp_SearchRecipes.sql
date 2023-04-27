CREATE PROCEDURE [dbo].[sp_SearchRecipes]
	@name NVARCHAR(200) = NULL,
	@tagNameList NVARCHAR(MAX) = NULL,
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
		UserId AS Id, Username,
		TagId AS Id, TagName AS Name, IsIngredient
	FROM VI_RecipeWithTags r
	WHERE 1 = dbo.udf_ShouldRecipeBeInResult(
			r.IsPublic, r.UserId, @userId, @includePublicRecipes, @includePrivateRecipesOfUser)
	ORDER BY CreationDate
	OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY;
	RETURN;
END
