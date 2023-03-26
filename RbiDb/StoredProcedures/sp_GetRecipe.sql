CREATE PROCEDURE [dbo].[sp_GetRecipe]
	@id int
AS
BEGIN
	SELECT r.Id, r.Name, r.Description, r.IsPublic, r.UserId,
		t.Id AS TagId, t.Name, t.IsIngredient. rt.Amount, rt.AmountUnit
	FROM Recipe r
	JOIN RecipeTag rt ON rt.RecipeId = r.Id
	JOIN Tag t ON t.Id = rt.TagId
	WHERE r.Id = @id;
END
