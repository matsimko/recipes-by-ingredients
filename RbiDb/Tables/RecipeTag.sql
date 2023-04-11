CREATE TABLE [dbo].[RecipeTag]
(
    [RecipeId] INT NOT NULL, 
    [TagId] INT NOT NULL, 
    /* concrete ingredients have additional data: */
    [OrderNum] INT NULL, 
    [Amount] DECIMAL(8, 2) NULL, 
    [AmountUnit] NVARCHAR(10) NULL,

    CONSTRAINT [PK_RecipeTag] PRIMARY KEY ([RecipeId], [TagId]),
    CONSTRAINT [FK_RecipeTag_Recipe] FOREIGN KEY ([RecipeId]) REFERENCES [Recipe]([Id]) ON DELETE CASCADE, 
    CONSTRAINT [FK_RecipeTag_Tag] FOREIGN KEY ([TagId]) REFERENCES [Tag]([Id]),
    CONSTRAINT [CK_RecipeTag_OrderNum] CHECK (OrderNum > 0),
)
