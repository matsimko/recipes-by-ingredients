CREATE PROCEDURE [dbo].[sp_GetRecipesForUser]
	@userId int
AS
BEGIN
	SELECT r.Id, r.Name, t.Id AS TagId, t.Name
	FROM Recipe r
	JOIN RecipeTag rt ON rt.RecipeId = r.Id
	JOIN Tag t ON t.Id = rt.TagId
	WHERE r.UserId = @userId;
END
