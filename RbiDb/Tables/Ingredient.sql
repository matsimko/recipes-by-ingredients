CREATE TABLE [dbo].[Ingredient]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(100) NOT NULL, 
    [UserId] INT NULL FOREIGN KEY REFERENCES [User], --null for globally available ingredients
    CONSTRAINT AK_User_Name_UserId UNIQUE ([Name], [UserId])
)
