CREATE PROCEDURE [dbo].[sp_GetTagsForUser]
	@userId int
AS
BEGIN
	SELECT Id, Name, IsIngredient
	FROM Tag t
	WHERE t.UserId = @userId;
END
