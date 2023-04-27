CREATE PROCEDURE [dbo].[sp_InsertRecipe]
	@name NVARCHAR(200),
	@description NVARCHAR(MAX),
	@isPublic BIT,
	@userId INT,
	@prepTimeMins INT,
	@cookTimeMins INT,
	@servings INT,
	@tags TagType READONLY,
	@instructions InstructionType READONLY
AS
BEGIN
	DECLARE @id INT;

	INSERT INTO Recipe (Name, Description, IsPublic, UserId, PrepTimeMins, CookTimeMins, Servings)
	VALUES (@name, @description, @isPublic, @userId, @prepTimeMins, @cookTimeMins, @servings);

	SELECT @id = SCOPE_IDENTITY();

	exec sp_SetTagsOfRecipe @tags, @id;
	exec sp_SetInstructionsOfRecipe @instructions, @id;

	SELECT @id;
END