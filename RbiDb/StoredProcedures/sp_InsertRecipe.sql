CREATE PROCEDURE [dbo].[sp_InsertRecipe]
	@name VARCHAR(200),
	@description nvarchar(MAX),
	@isPublic BIT,
	@userId INT
AS
BEGIN
	INSERT INTO Recipe (Name, Description, IsPublic, UserId)
	OUTPUT inserted.Id
	VALUES (@name, @description, @isPublic, @userId);
END