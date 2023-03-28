CREATE PROCEDURE [dbo].[sp_GetRecipeDetail]
	@id int
AS
BEGIN
	SELECT r.Id, r.Name, r.Description, r.IsPublic, 
		r.UserId, u.Username, u.IsAnonymous,
		t.Id AS TagId, t.Name, rt.IsIngredient, rt.Amount, rt.AmountUnit
	FROM Recipe r
	JOIN RecipeTag rt ON rt.RecipeId = r.Id
	JOIN Tag t ON t.Id = rt.TagId
	JOIN [User] u ON u.Id = r.UserId
	WHERE r.Id = @id;
END
