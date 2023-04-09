CREATE PROCEDURE [dbo].sp_RemoveInstructionFromRecipe
	@orderNum INT,
	@recipeId INT
AS
BEGIN
	DELETE FROM Instruction
	WHERE OrderNum = @orderNum AND RecipeId = @recipeId;
END