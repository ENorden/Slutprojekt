CREATE TABLE [rec].[Ingredient]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [RecID] INT NOT NULL references rec.Recipe(ID), 
    [Name] NVARCHAR(MAX) NOT NULL, 
    [Amount] FLOAT NOT NULL, 
    [Unit] NVARCHAR(50) NOT NULL
)
