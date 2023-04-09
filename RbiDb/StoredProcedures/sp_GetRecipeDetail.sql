CREATE PROCEDURE [dbo].[sp_GetRecipeDetail]
	@id int
AS
BEGIN
	SELECT Id, Name, Description, IsPublic, CreationDate, PrepTimeMins, CookTimeMins,
		UserId AS Id, Username,
		TagId AS Id, TagName AS Name, IsIngredient, Amount, AmountUnit
	FROM VI_RecipeWithTags
	WHERE Id = @id;
END
