CREATE TABLE [rec].[Recipe2Category]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [RecID] INT NOT NULL references rec.Recipe(ID), 
    [CatID] INT NOT NULL references rec.Category(Id)
)
