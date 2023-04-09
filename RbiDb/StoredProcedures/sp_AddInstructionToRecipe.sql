CREATE PROCEDURE [dbo].sp_AddInstructionToRecipe
	@text NVARCHAR(MAX),
	@orderNum INT,
	@recipeId INT
AS
BEGIN
	INSERT INTO Instruction(Text, OrderNum, RecipeId)
	VALUES (@text, @orderNum, @recipeId);
END