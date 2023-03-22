﻿CREATE TABLE [dbo].[Recipe]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(200) NOT NULL, 
    [Description] NVARCHAR(MAX) NULL, 
    [IsPublic] BIT NOT NULL DEFAULT 0
)
