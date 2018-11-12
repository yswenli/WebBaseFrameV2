---------添加字段描述

EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'级别',
    @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE',
    @level1name = N'Area', @level2type = N'COLUMN', @level2name = N'Depth'
GO
-----------更改字段描述
EXEC [sys].[sp_updateextendedproperty] @name = N'MS_Description',
    @value = N'级别2', @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'Area', @level2type = N'COLUMN',
    @level2name = N'Depth'
GO

-----查询字段描述

SELECT  ID = NEWID() ,
        库名 = 'WEPM_OA' ,
        表名 = CONVERT(VARCHAR(50), d.name) ,
        字段名 = CONVERT(VARCHAR(100), a.name) ,
        字段说明 = CONVERT(VARCHAR(50), ISNULL(g.[value], ''))
FROM    dbo.syscolumns a
        LEFT JOIN dbo.systypes b ON a.xusertype = b.xusertype
        INNER JOIN dbo.sysobjects d ON a.id = d.id
                                       AND d.xtype = 'U'
                                       AND d.name <> 'dtproperties'
        LEFT JOIN dbo.syscomments e ON a.cdefault = e.id
        LEFT JOIN sys.extended_properties g ON a.id = g.major_id
                                               AND a.colid = g.minor_id
        LEFT JOIN sys.extended_properties f ON d.id = f.major_id
                                               AND f.minor_id = 0
WHERE   d.name = 'Area'
                    
                    
