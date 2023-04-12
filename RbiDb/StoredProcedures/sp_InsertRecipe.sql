CREATE PROCEDURE [dbo].[sp_InsertRecipe]
	@name NVARCHAR(200),
	@description NVARCHAR(MAX),
	@isPublic BIT,
	@userId INT,
	@prepTimeMins INT,
	@cookTimeMins INT,
	@servings INT
AS
BEGIN
	INSERT INTO Recipe (Name, Description, IsPublic, UserId, PrepTimeMins, CookTimeMins, Servings)
	OUTPUT inserted.Id
	VALUES (@name, @description, @isPublic, @userId, @prepTimeMins, @cookTimeMins, @servings);
END