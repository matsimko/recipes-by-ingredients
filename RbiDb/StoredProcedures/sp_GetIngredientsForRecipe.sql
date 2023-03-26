CREATE PROCEDURE [dbo].[sp_GetIngredientsForRecipe]
	@recipeId Int
AS
BEGIN
	SELECT i.Id, i.Name, ui.Amount, ui.AmountUnit
	FROM UsedIngredient ui
	JOIN Ingredient i ON i.Id = ui.IngredientId
	WHERE ui.RecipeId = @recipeId;
END
