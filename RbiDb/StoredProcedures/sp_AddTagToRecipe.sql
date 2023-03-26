CREATE PROCEDURE [dbo].[sp_AddTagToRecipe]
	@name VARCHAR(100),
	@recipeId INT,
	@userId INT,
	@isIngredient BIT = 0,
	@amount DECIMAL(8,2) = NULL,
	@amountUnit VARCHAR(10) = NULL 
AS
BEGIN
	DECLARE @tagId INT;

	SELECT @tagId = Id
	FROM Tag 
	WHERE Name = @name AND (UserId IS NULL OR UserId = @userId);

	IF @tagId IS NULL
	BEGIN
		DECLARE @InsertedId TABLE (Id INT);

		INSERT INTO Tag (Name, IsIngredient, UserId)
		OUTPUT inserted.Id INTO @InsertedId
		VALUES (@name, @isIngredient, @userId);

		SELECT @tagId = Id FROM @InsertedId;
	END

	INSERT INTO RecipeTag (RecipeId, TagId, Amount, AmountUnit)
	VALUES (@recipeId, @tagId, @amount, @amountUnit);

END