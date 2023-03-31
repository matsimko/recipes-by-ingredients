﻿CREATE PROCEDURE [dbo].[sp_SearchRecipesByTags]
	@tagNameList VARCHAR(MAX),
	@userId INT = NULL,
	@includePrivateRecipesOfUser BIT = 1,
	@includePublicRecipes BIT = 1,
	@exactMatch BIT = 0,
	@offset INT = 0,
	@limit INT = 100
AS
BEGIN
	CREATE TABLE #TagsToMatch
	(
		[Name] VARCHAR(100)
	);
	INSERT INTO #TagsToMatch
	SELECT * FROM string_split(@tagNameList, ',');

	DECLARE @minTagsToMatch INT = 1;
	IF @exactMatch = 1
	BEGIN
		SELECT @minTagsToMatch = COUNT(*)
		FROM #TagsToMatch;
	END;

	WITH BestRecipeMatches AS 
	(
		SELECT r.Id
		FROM Recipe r
		JOIN RecipeTag rt ON rt.RecipeId = r.Id
		JOIN Tag t ON t.Id = rt.TagId
		--Temporary solution until SQL Server instance with full-text search is used
		JOIN #TagsToMatch ttm ON LOWER(t.Name) IN (LOWER(ttm.Name), LOWER(ttm.Name) + 's', LOWER(ttm.Name) + 'es')
		WHERE 1 = dbo.udf_ShouldRecipeBeInResult(
			r.IsPublic, r.UserId, @userId, @includePublicRecipes, @includePrivateRecipesOfUser)
		GROUP BY r.Id, r.CreationDate
		HAVING COUNT(*) >= @minTagsToMatch
		ORDER BY COUNT(*) DESC, r.CreationDate
		OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY
	)
	SELECT r.Id, Name, CreationDate,
		UserId AS Id, Username, IsAnonymous,
		TagId AS Id, TagName AS Name
	FROM VI_RecipeWithTags r
	JOIN BestRecipeMatches brm ON r.Id = brm.Id
END
