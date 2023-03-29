CREATE PROCEDURE [dbo].[sp_DeleteRecipe]
	@id INT
AS
BEGIN
	DELETE FROM Recipe
	WHERE Id = @id;
END