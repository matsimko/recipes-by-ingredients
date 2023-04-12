/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

IF NOT EXISTS (SELECT * FROM [User] WHERE Id = 1)
BEGIN
    INSERT INTO [User] (Id, Username) VALUES (1, 'TestUser');

    DECLARE @rId INT;
    INSERT INTO Recipe (Name, UserId, Servings)
    VALUES ('R1', 1, 2); 

    SELECT @rId = SCOPE_IDENTITY();
    exec sp_AddTagToRecipe 'T1', @rId;
    exec sp_AddTagToRecipe 'T2', @rId;
    exec sp_AddTagToRecipe 'I1', @rId, 1, 100, 'g';
    exec sp_AddTagToRecipe 'I2', @rId, 1, 200, 'ml';

    INSERT INTO Recipe (Name, UserId, Servings)
    VALUES ('R2', 1, NULL); 

    SELECT @rId = SCOPE_IDENTITY();
    exec sp_AddTagToRecipe 'T2', @rId;
END
