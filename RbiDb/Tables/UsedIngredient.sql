CREATE TABLE [dbo].[UsedIngredient]
(
	[IngredientId] INT NOT NULL,
	[RecipeId] INT NOT NULL,
    [Amount] DECIMAL(8, 2) NULL, 

    CONSTRAINT [PK_UsedIngredient] PRIMARY KEY (IngredientId, RecipeId),
    CONSTRAINT [FK_UsedIngredient_Recipe] FOREIGN KEY (RecipeId) REFERENCES Recipe(Id), 
    CONSTRAINT [FK_UsedIngredient_Ingredient] FOREIGN KEY (IngredientId) REFERENCES Ingredient(Id), 
)
