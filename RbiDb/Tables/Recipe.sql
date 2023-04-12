CREATE TABLE [dbo].[Recipe]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(200) NOT NULL, 
    [Description] NVARCHAR(MAX) NULL, 
    [IsPublic] BIT NOT NULL DEFAULT 0,
    [UserId] INT NULL,
    [Servings] INT NULL,
    [PrepTimeMins] INT NULL,
    [CookTimeMins] INT NULL,
    [CreationDate] DATETIME2 NOT NULL DEFAULT GETDATE(), 

    CONSTRAINT [FK_Recipe_User] FOREIGN KEY ([UserId]) REFERENCES [User]([Id]), 
    CONSTRAINT [CK_Recipe_Name] CHECK (LEN([Name]) > 0),

)
