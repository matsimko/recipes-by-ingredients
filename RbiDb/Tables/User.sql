CREATE TABLE [dbo].[User]
(
	[Id] INT NOT NULL PRIMARY KEY,
	Username VARCHAR(100) NOT NULL,

    CONSTRAINT [AK_User_Username] UNIQUE ([Username]), 
    CONSTRAINT [CK_User_Username] CHECK (LEN(Username) > 0)
)
