CREATE PROCEDURE [dbo].[sp_GetRecipesForUser]
	@userId int
AS
BEGIN
	SELECT r.Id, r.Name,
		r.UserId, u.Username, u.IsAnonymous,	
		t.Id AS TagId, t.Name
	FROM Recipe r
	JOIN RecipeTag rt ON rt.RecipeId = r.Id
	JOIN Tag t ON t.Id = rt.TagId
	JOIN [User] u ON u.Id = r.UserId
	WHERE r.UserId = @userId;
END
