USE MASTER

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'Tasks')
BEGIN
	CREATE DATABASE Tasks;
END
ELSE
	PRINT N'Database_Exist.'; 
GO

IF NOT EXISTS (SELECT * FROM [Tasks].sys.objects 
	WHERE object_id = OBJECT_ID(N'[Tasks].[dbo].[SimpleTasks]') AND type in (N'U'))

BEGIN
CREATE TABLE [Tasks].[dbo].[SimpleTasks](
    Id int IDENTITY(1,1) PRIMARY KEY,
    Description VARCHAR(50),
    IsComplete bit
)
END
ELSE
	PRINT N'Table_Exist.'; 
GO
GO