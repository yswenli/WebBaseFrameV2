CREATE FUNCTION GetPinYin ( @str VARCHAR(500) )
RETURNS VARCHAR(500)
AS
    BEGIN  
        DECLARE @cyc INT ,
            @length INT ,
            @str1 VARCHAR(100) ,
            @charcate VARBINARY(20)  
        SET @cyc = 1--从第几个字开始取  
        SET @length = LEN(@str)--输入汉字的长度  
        SET @str1 = ''--用于存放返回值  
        WHILE @cyc <= @length
            BEGIN    
                SELECT  @charcate = CAST(SUBSTRING(@str, @cyc, 1) AS VARBINARY)--每次取出一个字并将其转变成二进制,便于与GBK编码表进行比较  
 
                IF @charcate >= 0XB0A1
                    AND @charcate <= 0XB0C4
                    SET @str1 = @str1 + 'A'--说明此汉字的首字母为A,以下同上  
                ELSE
                    IF @charcate >= 0XB0C5
                        AND @charcate <= 0XB2C0
                        SET @str1 = @str1 + 'B'    
                    ELSE
                        IF @charcate >= 0XB2C1
                            AND @charcate <= 0XB4ED
                            SET @str1 = @str1 + 'C'  
                        ELSE
                            IF @charcate >= 0XB4EE
                                AND @charcate <= 0XB6E9
                                SET @str1 = @str1 + 'D'  
                            ELSE
                                IF @charcate >= 0XB6EA
                                    AND @charcate <= 0XB7A1
                                    SET @str1 = @str1 + 'E'  
                                ELSE
                                    IF @charcate >= 0XB7A2
                                        AND @charcate <= 0XB8C0
                                        SET @str1 = @str1 + 'F'  
                                    ELSE
                                        IF @charcate >= 0XB8C1
                                            AND @charcate <= 0XB9FD
                                            SET @str1 = @str1 + 'G'  
                                        ELSE
                                            IF @charcate >= 0XB9FE
                                                AND @charcate <= 0XBBF6
                                                SET @str1 = @str1 + 'H'  
                                            ELSE
                                                IF @charcate >= 0XBBF7
                                                    AND @charcate <= 0XBFA5
                                                    SET @str1 = @str1 + 'J'  
                                                ELSE
                                                    IF @charcate >= 0XBFA6
                                                        AND @charcate <= 0XC0AB
                                                        SET @str1 = @str1
                                                            + 'K'  
                                                    ELSE
                                                        IF @charcate >= 0XC0AC
                                                            AND @charcate <= 0XC2E7
                                                            SET @str1 = @str1
                                                              + 'L'  
                                                        ELSE
                                                            IF @charcate >= 0XC2E8
                                                              AND @charcate <= 0XC4C2
                                                              SET @str1 = @str1
                                                              + 'M'  
                                                            ELSE
                                                              IF @charcate >= 0XC4C3
                                                              AND @charcate <= 0XC5B5
                                                              SET @str1 = @str1
                                                              + 'N'  
                                                              ELSE
                                                              IF @charcate >= 0XC5B6
                                                              AND @charcate <= 0XC5BD
                                                              SET @str1 = @str1
                                                              + 'O'  
                                                              ELSE
                                                              IF @charcate >= 0XC5BE
                                                              AND @charcate <= 0XC6D9
                                                              SET @str1 = @str1
                                                              + 'P'  
                                                              ELSE
                                                              IF @charcate >= 0XC6DA
                                                              AND @charcate <= 0XC8BA
                                                              SET @str1 = @str1
                                                              + 'Q'  
                                                              ELSE
                                                              IF @charcate >= 0XC8BB
                                                              AND @charcate <= 0XC8F5
                                                              SET @str1 = @str1
                                                              + 'R'  
                                                              ELSE
                                                              IF @charcate >= 0XC8F6
                                                              AND @charcate <= 0XCBF9
                                                              SET @str1 = @str1
                                                              + 'S'  
                                                              ELSE
                                                              IF @charcate >= 0XCBFA
                                                              AND @charcate <= 0XCDD9
                                                              SET @str1 = @str1
                                                              + 'T'  
                                                              ELSE
                                                              IF @charcate >= 0XCDDA
                                                              AND @charcate <= 0XCEF3
                                                              SET @str1 = @str1
                                                              + 'W'  
                                                              ELSE
                                                              IF @charcate >= 0XCEF4
                                                              AND @charcate <= 0XD1B8
                                                              SET @str1 = @str1
                                                              + 'X'  
                                                              ELSE
                                                              IF @charcate >= 0XD1B9
                                                              AND @charcate <= 0XD4D0
                                                              SET @str1 = @str1
                                                              + 'Y'  
                                                              ELSE
                                                              IF @charcate >= 0XD4D1
                                                              AND @charcate <= 0XD7F9
                                                              SET @str1 = @str1
                                                              + 'Z'  
                SET @cyc = @cyc + 1--取出输入汉字的下一个字  
            END  
        RETURN @str1--返回输入汉字的首字母  
    END --测试数据  


select dbo.GetPinYin(name))