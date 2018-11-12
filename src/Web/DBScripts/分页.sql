CREATE PROCEDURE proc_getList
    (
      @pageIndex INT ,
      @pageSize INT ,
      @tableName NVARCHAR(20)
    )
AS
    BEGIN
        EXEC('select * from (select row_number() over (order by id desc) as  row, * from '+@tableName+' AS a) AS b where b.row between ((' + @pageIndex + ' - 1) * ' + @pageSize + ' + 1) and ((' + @pageIndex + ' - 1 )*' + @pageSize + '+' + @pageSize + ')')
    END