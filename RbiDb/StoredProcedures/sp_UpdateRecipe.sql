CREATE PROCEDURE [dbo].[sp_UpdateRecipe]
	@id INT,
	@name VARCHAR(200),
	@description nvarchar(MAX),
	@isPublic BIT
AS
BEGIN
	UPDATE Recipe
	SET Name = @name, Description = @description, IsPublic = @isPublic
	WHERE Id = @id;
END