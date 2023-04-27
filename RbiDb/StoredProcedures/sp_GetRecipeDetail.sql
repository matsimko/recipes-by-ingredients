CREATE PROCEDURE [dbo].[sp_GetRecipeDetail]
	@id int
AS
BEGIN
	SELECT Id, Name, Description, IsPublic, CreationDate, PrepTimeMins, CookTimeMins, Servings,
		UserId AS Id, Username,
		TagId AS Id, TagName AS Name, IsIngredient, OrderNum, Amount, AmountUnit
	FROM VI_RecipeWithTags
	WHERE Id = @id
	ORDER BY OrderNum;
END
