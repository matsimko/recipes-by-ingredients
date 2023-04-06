CREATE PROCEDURE [dbo].[sp_SearchTags]
	@prefix NVARCHAR(100)
AS
BEGIN
	SELECT *
	FROM Tag t
	WHERE t.Name LIKE @prefix + '%';
END
