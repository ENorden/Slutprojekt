CREATE TABLE [rec].[StepByStep]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [RecID] INT NOT NULL references rec.Recipe(Id), 
    [Instruction] NVARCHAR(MAX) NOT NULL, 
    [SortOrder] INT NOT NULL
)
