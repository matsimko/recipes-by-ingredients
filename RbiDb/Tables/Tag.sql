CREATE TABLE [dbo].[Tag]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(100) NOT NULL, 
    [IsIngredient] BIT NOT NULL DEFAULT 0,
    [UserId] INT NULL FOREIGN KEY REFERENCES [User], --null for globally available tags
)
