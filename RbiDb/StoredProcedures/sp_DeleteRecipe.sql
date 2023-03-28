CREATE PROCEDURE [dbo].[spRecipe_Delete]
	@id INT
AS
BEGIN
	DELETE FROM Recipe
	WHERE Id = @id;
END