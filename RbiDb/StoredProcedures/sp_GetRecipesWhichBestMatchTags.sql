CREATE PROCEDURE [dbo].[sp_GetRecipesWhichBestMatchTags]
	@tagIdList VARCHAR(MAX),
	@userID INT,
	@offset INT = 0,
	@maxRows INT = 100
AS
BEGIN
	CREATE TABLE #TagsToMatch
	(
		Id INT
	);
	INSERT INTO #TagsToMatch
	SELECT * FROM string_split(@tagIdList, ',');

	WITH BestRecipeMatches AS 
	(
		SELECT ui.RecipeId--, COUNT(*) AS MatchCount
		FROM RecipeTag rt
		JOIN #TagsToMatch ttm ON ttm.Id = rt.IngredientId
		WHERE r.UserId IS NULL OR r.UserId = @userID
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
