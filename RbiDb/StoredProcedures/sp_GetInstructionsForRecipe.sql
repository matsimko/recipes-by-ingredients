﻿CREATE PROCEDURE [dbo].[sp_GetInstructionsForRecipe]
	@recipeId INT
AS
BEGIN
	SELECT i.Text
	FROM Instruction i
	WHERE i.RecipeId = @recipeId
	ORDER BY i.OrderNum;
END