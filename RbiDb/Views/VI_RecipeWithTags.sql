CREATE VIEW [dbo].[VI_RecipeWithTags] AS
SELECT r.Id, r.Name, r.CreationDate, r.IsPublic, r.Description,
		r.UserId, u.Username, u.IsAnonymous,
		t.Id AS TagId, t.Name AS TagName, rt.IsIngredient, rt.Amount, rt.AmountUnit
FROM Recipe r
JOIN RecipeTag rt ON rt.RecipeId = r.Id
JOIN Tag t ON t.Id = rt.TagId
JOIN [User] u ON u.Id = r.UserId
