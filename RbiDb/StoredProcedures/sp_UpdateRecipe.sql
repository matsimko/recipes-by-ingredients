CREATE PROCEDURE [dbo].[sp_UpdateRecipe]
	@id INT,
	@name NVARCHAR(200),
	@description NVARCHAR(MAX),
	@isPublic BIT,
	@prepTimeMins INT,
	@cookTimeMins INT,
	@servings INT
AS
BEGIN
	UPDATE Recipe
	SET Name = @name, Description = @description, IsPublic = @isPublic,
		PrepTimeMins = @prepTimeMins, CookTimeMins = @cookTimeMins, Servings = @servings
	WHERE Id = @id;
END