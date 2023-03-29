CREATE PROCEDURE [dbo].sp_SearchRecipesByName
	@name VARCHAR(200),
	@userId INT = NULL,
	@includePrivateRecipesOfUser BIT = 1,
	@includePublicRecipes BIT = 1,
	@offset INT = 0,
	@maxRows INT = 100
AS
BEGIN
	SELECT r.Id, Name, CreationDate,
		UserId, Username, IsAnonymous,
		TagId, TagName AS Name
	FROM VI_RecipeWithTags r
	WHERE 1 = dbo.udf_ShouldRecipeBeInResult(
			r.IsPublic, r.UserId, @userId, @includePublicRecipes, @includePrivateRecipesOfUser) AND
			--Temporary solution until SQL Server instance with full-text search is used
			LOWER(@name) = LOWER(r.Name)
	ORDER BY r.CreationDate
	OFFSET @offset ROWS FETCH NEXT @maxRows ROWS ONLY;
END
