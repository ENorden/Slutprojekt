CREATE TABLE [rec].[Follower]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[UserID] NVARCHAR(450) NOT NULL references dbo.AspNetUsers(Id), 
	[FollowerID] NVARCHAR(450) NOT NULL references dbo.AspNetUsers(Id)
)
