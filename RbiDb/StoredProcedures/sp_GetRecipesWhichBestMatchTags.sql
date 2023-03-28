CREATE PROCEDURE [dbo].[sp_GetRecipesWhichBestMatchTags]
	@tagNameList VARCHAR(MAX),
	@userID INT,
	@includePublicRecipes BIT = 1,
	@offset INT = 0,
	@maxRows INT = 100
AS
BEGIN
	CREATE TABLE #TagsToMatch
	(
		[Name] VARCHAR(100)
	);
	INSERT INTO #TagsToMatch
	SELECT * FROM string_split(@tagNameList, ',');

	WITH BestRecipeMatches AS 
	(
		SELECT r.Id
		FROM Recipe r
		JOIN RecipeTag rt ON rt.RecipeId = r.Id
		JOIN Tag t ON t.Id = rt.TagId
		JOIN #TagsToMatch ttm ON t.Name IN (ttm.Name, ttm.Name + 's'. ttm.Name + 'es')
		WHERE r.UserId = @userID OR (@includePublicRecipes = 1 AND r.IsPublic = 1)
		GROUP BY ttm.RecipeId
		HAVING COUNT(*) > 0
		ORDER BY COUNT(*)
		OFFSET @Offset ROWS FETCH NEXT @maxRows ROWS ONLY
	)
	SELECT r.Id, r.Name,
		r.UserId, u.Username, u.IsAnonymous,
		t.Id AS TagId, t.Name
	FROM Recipe r
	JOIN BestRecipeMatches brm ON r.Id = brm.Id
	JOIN RecipeTag rt ON rt.RecipeId = r.Id
	JOIN Tag t ON t.Id = rt.TagId
	JOIN [User] u ON u.Id = r.UserId

END
