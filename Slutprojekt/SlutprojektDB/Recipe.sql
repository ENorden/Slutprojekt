﻿CREATE TABLE [rec].[Recipe]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [UserID] nvarchar(450) NULL references dbo.AspNetUsers(Id),
	[Img] nvarchar(max) NOT NULL,
	[Title] nvarchar(max) NULL
)
