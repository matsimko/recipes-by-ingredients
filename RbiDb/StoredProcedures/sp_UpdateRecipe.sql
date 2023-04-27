CREATE PROCEDURE [dbo].[sp_UpdateRecipe]
	@id INT,
	@name NVARCHAR(200),
	@description NVARCHAR(MAX),
	@isPublic BIT,
	@prepTimeMins INT,
	@cookTimeMins INT,
	@servings INT,
	@tags TagType READONLY,
	@instructions InstructionType READONLY
AS
BEGIN
	UPDATE Recipe
	SET Name = @name, Description = @description, IsPublic = @isPublic,
		PrepTimeMins = @prepTimeMins, CookTimeMins = @cookTimeMins, Servings = @servings
	WHERE Id = @id;

	exec sp_SetTagsOfRecipe @tags, @id;
	exec sp_SetInstructionsOfRecipe @instructions, @id;
END