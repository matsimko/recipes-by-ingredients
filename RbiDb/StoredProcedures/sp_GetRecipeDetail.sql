CREATE PROCEDURE [dbo].[sp_GetRecipeDetail]
	@id int
AS
BEGIN
	SELECT Id, Name, Description, IsPublic, CreationDate,
		UserId, Username, IsAnonymous,
		TagId, TagName AS Name, IsIngredient, Amount, AmountUnit
	FROM VI_RecipeWithTags
	WHERE Id = @id;
END
