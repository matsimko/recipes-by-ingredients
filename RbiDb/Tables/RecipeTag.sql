CREATE TABLE [dbo].[RecipeTag]
(
    [RecipeId] INT NOT NULL, 
    [TagId] INT NOT NULL, 
    /* recipes can specify amount if the tag is an ingredient
    but I am not forbidding it in the case of non-ingredient tags (a trigger would have to be used otherwise) */
    [Amount] DECIMAL(8, 2) NULL, 
    [AmountUnit] NCHAR(10) NULL,

    CONSTRAINT [PK_RecipeTag] PRIMARY KEY ([RecipeId], [TagId]),
    CONSTRAINT [FK_RecipeTag_Recipe] FOREIGN KEY ([RecipeId]) REFERENCES [Recipe]([Id]) ON DELETE CASCADE, 
    CONSTRAINT [FK_RecipeTag_Tag] FOREIGN KEY ([TagId]) REFERENCES [Tag]([Id])
)
