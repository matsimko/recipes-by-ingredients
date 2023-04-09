CREATE TABLE [dbo].[Instruction]
(
	[RecipeId] INT NOT NULL,
	[OrderNum] INT NOT NULL, 
    [Text] NVARCHAR(MAX) NOT NULL,

    CONSTRAINT [PK_Instruction] PRIMARY KEY ([RecipeId], [OrderNum]),
    CONSTRAINT [FK_Instruction_Recipe] FOREIGN KEY ([RecipeId]) REFERENCES [Recipe]([Id]) ON DELETE CASCADE, 
    CONSTRAINT [CK_Instruction_OrderNum] CHECK (OrderNum > 0), 
)
