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
		SELECT ui.RecipeId--, COUNT(*) AS MatchCount
		FROM RecipeTag rt
		JOIN Tag t ON t.Id = rt.TagId
		JOIN #TagsToMatch ttm ON t.Name IN (ttm.Name, ttm.Name + 's'. ttm.Name + 'es')
		WHERE r.UserId = @userID OR (@includePublicRecipes = 1 AND r.IsPublic = 1)
		GROUP BY ttm.RecipeId
		HAVING COUNT(*) > 0
		ORDER BY COUNT(*)
		OFFSET @Offset ROWS FETCH NEXT @maxRows ROWS ONLY
	)
	SELECT r.Id, r.Name, t.Id AS TagId, t.Name
	FROM Recipe r
	JOIN BestRecipeMatches brm ON r.Id = brm.RecipeId
	JOIN RecipeTag rt ON rt.RecipeId = r.Id
	JOIN Tag t ON t.Id = rt.TagId

END
