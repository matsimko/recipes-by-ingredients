CREATE PROCEDURE [dbo].[sp_AddTagToRecipe]
	@name NVARCHAR(100),
	@recipeId INT,
	@isIngredient BIT = 0,
	@amount DECIMAL(8,2) = NULL,
	@amountUnit VARCHAR(10) = NULL 
AS
BEGIN
	DECLARE @tagId INT;

	SELECT @tagId = Id
	FROM Tag 
	WHERE Name = @name;

	IF @tagId IS NULL
	BEGIN
		DECLARE @InsertedId TABLE (Id INT);

		INSERT INTO Tag (Name, IsIngredient)
		OUTPUT inserted.Id INTO @InsertedId
		VALUES (@name, @isIngredient);

		SELECT @tagId = Id FROM @InsertedId;
	END

	INSERT INTO RecipeTag (RecipeId, TagId, Amount, AmountUnit)
	VALUES (@recipeId, @tagId, @isIngredient, @amount, @amountUnit);

	SELECT @tagId;
END