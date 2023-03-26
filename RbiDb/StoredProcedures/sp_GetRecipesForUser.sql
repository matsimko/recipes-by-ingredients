CREATE PROCEDURE [dbo].[sp_GetRecipesForUser]
	@userId int
AS
BEGIN
	SELECT r.Id, r.Name, i.Id AS IngredientId, i.Name
	FROM Recipe r
	JOIN UsedIngredient ui ON ui.RecipeId = r.Id
	JOIN Ingredient i ON i.Id = ui.IngredientId
	WHERE r.UserId = @userId;
END
