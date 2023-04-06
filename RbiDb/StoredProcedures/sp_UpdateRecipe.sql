CREATE PROCEDURE [dbo].[sp_UpdateRecipe]
	@id INT,
	@name NVARCHAR(200),
	@description NVARCHAR(MAX),
	@isPublic BIT
AS
BEGIN
	UPDATE Recipe
	SET Name = @name, Description = @description, IsPublic = @isPublic
	WHERE Id = @id;
END