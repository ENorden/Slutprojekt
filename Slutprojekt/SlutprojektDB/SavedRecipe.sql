CREATE TABLE [rec].[SavedRecipe]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [RecID] INT NOT NULL references rec.Recipe(ID), 
    [UserID] NVARCHAR(450) NOT NULL references dbo.AspNetUSers(Id)
)
