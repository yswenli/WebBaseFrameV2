CREATE FUNCTION [md5] ( @src VARCHAR(255) -- 源字符串
                              )
RETURNS VARCHAR(255)
    WITH EXECUTE AS CALLER
AS
    BEGIN
    -- 存放md5加密串(ox)
        DECLARE @smd5 VARCHAR(34)
    -- 加密字符串
        SELECT  @smd5 = UPPER(RIGHT(sys.fn_VarBinToHexStr(HASHBYTES('MD5',
                                                              @src)), 32))    --32位
    -- 返回加密串
        RETURN @smd5
    END