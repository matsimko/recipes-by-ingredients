CREATE PROCEDURE [dbo].[sp_GetRecipesWhichMatchIngredientsBest]
	@IngredientIdList VARCHAR(MAX),
	@Offset INT = 0,
	@MaxRows INT = 100
AS
BEGIN
	CREATE TABLE #IngredientsToMatch
	(
		Id INT
	);
	INSERT INTO #IngredientsToMatch
	SELECT * FROM string_split(@IngredientIdList, ',');

	WITH BestRecipeMatches AS 
	(
		SELECT ui.RecipeId, COUNT(*) AS MatchCount
		FROM UsedIngredient ui
		--JOIN Ingredient i ON i.Id = ui.IngredientId
		JOIN #IngredientsToMatch itm ON itm.Id = ui.IngredientId
		GROUP BY ui.RecipeId
		HAVING COUNT(*) > 0
		ORDER BY COUNT(*)
		OFFSET @Offset ROWS FETCH NEXT @MaxRows ROWS ONLY
	)
	SELECT r.Id, r.Name, i.Name AS InredientName
	FROM Recipe r
	JOIN BestRecipeMatches brm ON r.Id = brm.RecipeId
	JOIN UsedIngredient ui ON ui.RecipeId = r.Id
	JOIN Ingredient i ON ui.IngredientId= i.Id;

END
