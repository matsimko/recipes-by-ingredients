CREATE PROCEDURE [dbo].[sp_GetRecipe]
	@id int
AS
BEGIN
	SELECT r.Id, r.Name, r.Description, r.IsPublic, r.CreationDate,
		r.UserId AS Id, u.Username, u.IsAnonymous
	FROM Recipe r
	LEFT JOIN [User] u ON u.Id = r.UserId
	WHERE r.Id = @id;
END
