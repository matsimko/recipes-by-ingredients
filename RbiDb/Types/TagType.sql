CREATE TYPE [dbo].TagType AS TABLE
(
    [Name] NVARCHAR(100) NOT NULL,
    [IsIngredient] BIT NOT NULL DEFAULT 0,
    [OrderNum] INT NULL, 
    [Amount] DECIMAL(8, 2) NULL, 
    [AmountUnit] NVARCHAR(10) NULL
)
