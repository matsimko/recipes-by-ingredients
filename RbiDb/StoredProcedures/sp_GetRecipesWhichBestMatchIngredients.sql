CREATE PROCEDURE [dbo].[sp_GetRecipesWhichBestMatchIngredients]
	@ingredientIdList VARCHAR(MAX),
	@userID INT,
	@offset INT = 0,
	@maxRows INT = 100
AS
BEGIN
	CREATE TABLE #IngredientsToMatch
	(
		Id INT
	);
	INSERT INTO #IngredientsToMatch
	SELECT * FROM string_split(@ingredientIdList, ',');

	WITH BestRecipeMatches AS 
	(
		SELECT ui.RecipeId--, COUNT(*) AS MatchCount
		FROM UsedIngredient ui
		--JOIN Ingredient i ON i.Id = ui.IngredientId
		JOIN #IngredientsToMatch itm ON itm.Id = ui.IngredientId
		WHERE r.UserId IS NULL OR r.UserId = @userID
		GROUP BY ui.RecipeId
		HAVING COUNT(*) > 0
		ORDER BY COUNT(*)
		OFFSET @offset ROWS FETCH NEXT @maxRows ROWS ONLY
	)
	SELECT r.Id, r.Name, i.Id AS IngredientId, i.Name
	FROM Recipe r
	JOIN BestRecipeMatches brm ON r.Id = brm.RecipeId
	JOIN UsedIngredient ui ON ui.RecipeId = r.Id
	JOIN Ingredient i ON ui.IngredientId= i.Id;

END
