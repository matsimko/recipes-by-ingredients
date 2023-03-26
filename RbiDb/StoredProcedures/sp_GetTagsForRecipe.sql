CREATE PROCEDURE [dbo].[sp_GetTagsForRecipe]
	@recipeId Int
AS
BEGIN
	SELECT t.Id, t.Name
	FROM RecipeTag rt
	JOIN Tag t ON t.Id = rt.TagId
	WHERE rt.RecipeId = @recipeId;
END
