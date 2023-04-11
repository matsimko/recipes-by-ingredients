CREATE TABLE [dbo].[Tag]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(100) NOT NULL,
    [IsIngredient] BIT NOT NULL DEFAULT 0,

    CONSTRAINT [AK_Tag_Name] UNIQUE ([Name], [IsIngredient]), 
    CONSTRAINT [CK_Tag_Name] CHECK (LEN([Name]) > 0), 
)
