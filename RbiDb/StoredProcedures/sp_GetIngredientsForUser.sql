CREATE PROCEDURE [dbo].[sp_GetIngredientsForUser]
	@userId int
AS
BEGIN
	SELECT Id, Name
	FROM Ingredient i
	WHERE i.UserId = @userId;
END
