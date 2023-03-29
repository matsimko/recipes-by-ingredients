﻿CREATE TABLE [dbo].[Tag]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(100) NOT NULL, 

    CONSTRAINT [AK_Tag_Name] UNIQUE ([Name]), 
    CONSTRAINT [CK_Tag_Name] CHECK (LEN([Name]) > 0), 
)
