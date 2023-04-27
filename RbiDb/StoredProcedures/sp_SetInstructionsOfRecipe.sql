CREATE PROCEDURE [dbo].sp_SetInstructionsOfRecipe
	@instructions InstructionType READONLY,
	@recipeId INT
AS
BEGIN
	DELETE FROM Instruction
	WHERE RecipeId = @recipeId;

	INSERT INTO Instruction (Text, OrderNum, RecipeId)
	SELECT Text, OrderNum, @recipeId
	FROM @instructions;
END