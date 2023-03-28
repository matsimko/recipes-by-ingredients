CREATE PROCEDURE [dbo].[sp_RemoveTagFromRecipe]
	@tagId INT,
	@recipeId INT
AS
BEGIN
	DELETE FROM RecipeTag
	WHERE TagId = @tagId AND RecipeId = @recipeId;
END