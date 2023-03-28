CREATE PROCEDURE [dbo].[spRecipe_Insert]
	@name VARCHAR(200),
	@description nvarchar(MAX),
	@isPublic BIT,
	@userId INT
AS
BEGIN
	INSERT INTO Recipe (Name, Description, IsPublic, UserId)
	VALUES (@name, @description, @isPublic, @userId);
END