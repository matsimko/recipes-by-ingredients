CREATE PROCEDURE [dbo].[sp_GetRecipe]
	@id int
AS
BEGIN
	SELECT r.Id, r.Name, r.Description, r.IsPublic, 
		r.UserId, u.Username, u.IsAnonymous
	FROM Recipe r
	JOIN [User] u ON u.Id = r.UserId
	WHERE r.Id = @id;
END
