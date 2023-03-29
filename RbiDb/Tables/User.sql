CREATE TABLE [dbo].[User]
(
	[Id] INT NOT NULL PRIMARY KEY,
	Username VARCHAR(100) NOT NULL,
	IsAnonymous BIT NOT NULL DEFAULT 0, 

    CONSTRAINT [AK_User_Username] UNIQUE ([Username]), 
    CONSTRAINT [CK_User_Username] CHECK (LEN(Username) > 0)
)
