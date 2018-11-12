--sqlserver2005
DUMP TRANSACTION [DB_IBD_v2012] WITH NO_LOG 
BACKUP LOG [DB_IBD_v2012] WITH NO_LOG 
DBCC SHRINKDATABASE([DB_IBD_v2012]) 



---sqlserver2008

USE [master]
GO
ALTER DATABASE DB_webBaseFrame SET RECOVERY SIMPLE WITH NO_WAIT
GO
ALTER DATABASE DB_webBaseFrame SET RECOVERY SIMPLE --简单模式
GO
USE DB_webBaseFrame
GO


DBCC SHRINKFILE (N'DB_webBaseFrame_log' , 11, TRUNCATEONLY)
GO

--这里的DNName_Log 如果不知道在sys.database_files里是什么名字的话，可以用以下的语句进行查询
USE DB_webBaseFrame

GO

SELECT file_id, name FROM sys.database_files;

GO

USE [master]
GO
ALTER DATABASE DB_webBaseFrame SET RECOVERY FULL WITH NO_WAIT
GO
ALTER DATABASE DB_webBaseFrame SET RECOVERY FULL --还原为完全模式
GO