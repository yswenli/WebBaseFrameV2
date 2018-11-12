--测试用
IF OBJECT_ID('[tb]') IS NOT NULL
    DROP TABLE [tb]
GO
CREATE TABLE tb
    (
      姓名 VARCHAR(10) ,
      课程 VARCHAR(10) ,
      分数 INT
    )
INSERT  INTO tb
VALUES  ( '张三', '语文', 74 )
INSERT  INTO tb
VALUES  ( '张三', '数学', 83 )
INSERT  INTO tb
VALUES  ( '张三', '物理', 93 )
INSERT  INTO tb
VALUES  ( '李四', '语文', 74 )
INSERT  INTO tb
VALUES  ( '李四', '数学', 84 )
INSERT  INTO tb
VALUES  ( '李四', '物理', 94 )
GO

--SQL SERVER 2000 动态SQL,指课程不止语文、数学、物理这三门课程。(以下同)
DECLARE @sql VARCHAR(8000)
SET @sql = 'select 姓名 '
SELECT  @sql = @sql + ' , max(case 课程 when ''' + 课程
        + ''' then 分数 else 0 end) [' + 课程 + ']'
FROM    ( SELECT DISTINCT
                    课程
          FROM      tb
        ) AS a
SET @sql = @sql + ' from tb group by 姓名'
EXEC(@sql) 
--通过动态构建@sql，得到如下脚本
SELECT  姓名 AS 姓名 ,
        MAX(CASE 课程
              WHEN '语文' THEN 分数
              ELSE 0
            END) 语文 ,
        MAX(CASE 课程
              WHEN '数学' THEN 分数
              ELSE 0
            END) 数学 ,
        MAX(CASE 课程
              WHEN '物理' THEN 分数
              ELSE 0
            END) 物理
FROM    tb
GROUP BY 姓名
