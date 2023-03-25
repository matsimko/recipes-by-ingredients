CREATE TABLE [dbo].[RecipeTag]
(
    [RecipeId] INT NOT NULL, 
    [TagId] INT NOT NULL, 

    CONSTRAINT [PK_RecipeTag] PRIMARY KEY ([RecipeId], [TagId]),
    CONSTRAINT [FK_RecipeTag_Recipe] FOREIGN KEY ([RecipeId]) REFERENCES [Recipe]([Id]) ON DELETE CASCADE, 
    CONSTRAINT [FK_RecipeTag_Tag] FOREIGN KEY ([TagId]) REFERENCES [Tag]([Id])
)
