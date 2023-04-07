CREATE PROCEDURE [dbo].sp_SearchRecipesByName
	@name NVARCHAR(200),
	@userId INT = NULL,
	@includePrivateRecipesOfUser BIT = 1,
	@includePublicRecipes BIT = 1,
	@offset INT = 0,
	@limit INT = 100
AS
BEGIN
	SELECT r.Id, Name, CreationDate,
		UserId AS Id, Username,
		TagId AS Id, TagName AS Name, IsIngredient, Amount, AmountUnit
	FROM VI_RecipeWithTags r
	WHERE 1 = dbo.udf_ShouldRecipeBeInResult(
			r.IsPublic, r.UserId, @userId, @includePublicRecipes, @includePrivateRecipesOfUser) AND
			--Temporary solution until SQL Server instance with full-text search is used
			@name = r.Name --the instance is set to be case insensitive, so LOWER() is not necessary
	ORDER BY r.CreationDate
	OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY;
END
