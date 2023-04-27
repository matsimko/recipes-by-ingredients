CREATE PROCEDURE [dbo].[sp_SetTagsOfRecipe]
	@tags TagType READONLY,
	@recipeId INT
AS
BEGIN
	DELETE FROM RecipeTag
	WHERE RecipeId = @recipeId;

	MERGE Tag AS Target
	USING @tags AS Source
	ON Source.Name = Target.Name AND Source.IsIngredient = Target.IsIngredient
	WHEN NOT MATCHED BY Target THEN
		INSERT (Name, IsIngredient)
		VALUES (Source.Name, Source.IsIngredient);

	INSERT INTO RecipeTag (RecipeId, TagId, Amount, AmountUnit, OrderNum)
	SELECT @recipeId, Tag.Id, t.Amount, t.AmountUnit, t.OrderNum
	FROM @tags t
	JOIN Tag ON Tag.Name = t.Name AND Tag.IsIngredient = t.IsIngredient
END