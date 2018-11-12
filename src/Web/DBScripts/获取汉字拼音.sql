CREATE FUNCTION [test_GetPinyin] ( @words NVARCHAR(2000) )
RETURNS VARCHAR(8000)
AS
    BEGIN 
        DECLARE @word NCHAR(1) 
        DECLARE @pinyin VARCHAR(8000) 
        DECLARE @i INT 
        DECLARE @words_len INT 
        DECLARE @unicode INT 
        SET @i = 1 
        SET @words = LTRIM(RTRIM(@words)) 
        SET @words_len = LEN(@words) 
        WHILE ( @i <= @words_len ) --—≠ª∑»°◊÷∑˚ 
            BEGIN 
                SET @word = SUBSTRING(@words, @i, 1) 
                SET @unicode = UNICODE(@word) 
                SET @pinyin = ISNULL(@pinyin + SPACE(1), '')
                    + ( CASE WHEN UNICODE(@word) BETWEEN 19968 AND 19968
                                  + 20901
                             THEN ( SELECT TOP 1
                                            py
                                    FROM    ( SELECT    'a' AS py ,
                                                        N'ÖÅ' AS word
                                              UNION ALL
                                              SELECT    'ai' ,
                                                        N'Ïa'
                                              UNION ALL
                                              SELECT    'an' ,
                                                        N'˜ˆ'
                                              UNION ALL
                                              SELECT    'ang' ,
                                                        N'·l'
                                              UNION ALL
                                              SELECT    'ao' ,
                                                        N'Úà'
                                              UNION ALL
                                              SELECT    'ba' ,
                                                        N'ôÒ'
                                              UNION ALL
                                              SELECT    'bai' ,
                                                        N'ÆB' --ÌvÉƒÆB 
                                              UNION ALL
                                              SELECT    'ban' ,
                                                        N'∞Í'
                                              UNION ALL
                                              SELECT    'bang' ,
                                                        N'Ê^'
                                              UNION ALL
                                              SELECT    'bao' ,
                                                        N'Ët'
                                              UNION ALL
                                              SELECT    'bei' ,
                                                        N'ˆÕ'
                                              UNION ALL
                                              SELECT    'ben' ,
                                                        N'›ô'
                                              UNION ALL
                                              SELECT    'beng' ,
                                                        N'Áa'
                                              UNION ALL
                                              SELECT    'bi' ,
                                                        N'¸Ñ'
                                              UNION ALL
                                              SELECT    'bian' ,
                                                        N'◊É'
                                              UNION ALL
                                              SELECT    'biao' ,
                                                        N'˜B'
                                              UNION ALL
                                              SELECT    'bie' ,
                                                        N'èï'
                                              UNION ALL
                                              SELECT    'bin' ,
                                                        N'ÙW'
                                              UNION ALL
                                              SELECT    'bing' ,
                                                        N'Ïh'
                                              UNION ALL
                                              SELECT    'bo' ,
                                                        N' N'
                                              UNION ALL
                                              SELECT    'bu' ,
                                                        N'≤æ'
                                              UNION ALL
                                              SELECT    'ca' ,
                                                        N'áÕ'
                                              UNION ALL
                                              SELECT    'cai' ,
                                                        N'Åk' --ønÅk 
                                              UNION ALL
                                              SELECT    'can' ,
                                                        N'†|'
                                              UNION ALL
                                              SELECT    'cang' ,
                                                        N'Ÿâ'
                                              UNION ALL
                                              SELECT    'cao' ,
                                                        N'¸è'
                                              UNION ALL
                                              SELECT    'ce' ,
                                                        N'∫u'
                                              UNION ALL
                                              SELECT    'cen' ,
                                                        N'∏í'
                                              UNION ALL
                                              SELECT    'ceng' ,
                                                        N'Åu' --≥Ä≥íçK™eÅu 
                                              UNION ALL
                                              SELECT    'cha' ,
                                                        N'‘å'
                                              UNION ALL
                                              SELECT    'chai' ,
                                                        N'á–'
                                              UNION ALL
                                              SELECT    'chan' ,
                                                        N'Óù'
                                              UNION ALL
                                              SELECT    'chang' ,
                                                        N'Ìo'
                                              UNION ALL
                                              SELECT    'chao' ,
                                                        N'”e'
                                              UNION ALL
                                              SELECT    'che' ,
                                                        N'†Ö'
                                              UNION ALL
                                              SELECT    'chen' ,
                                                        N'◊è'
                                              UNION ALL
                                              SELECT    'cheng' ,
                                                        N'≥”'
                                              UNION ALL
                                              SELECT    'chi' ,
                                                        N'˙u'
                                              UNION ALL
                                              SELECT    'chong' ,
                                                        N'„|'
                                              UNION ALL
                                              SELECT    'chou' ,
                                                        N'öé'
                                              UNION ALL
                                              SELECT    'chu' ,
                                                        N'¥£'
                                              UNION ALL
                                              SELECT    'chuai' ,
                                                        N'ıﬂ'
                                              UNION ALL
                                              SELECT    'chuan' ,
                                                        N'˙E'
                                              UNION ALL
                                              SELECT    'chuang' ,
                                                        N'êÌ'
                                              UNION ALL
                                              SELECT    'chui' ,
                                                        N'Óq'
                                              UNION ALL
                                              SELECT    'chun' ,
                                                        N'¥¿'
                                              UNION ALL
                                              SELECT    'chuo' ,
                                                        N'øW'
                                              UNION ALL
                                              SELECT    'ci' ,
                                                        N'ÜÔ' --ŸnÜÔ 
                                              UNION ALL
                                              SELECT    'cong' ,
                                                        N'÷Å'
                                              UNION ALL
                                              SELECT    'cou' ,
                                                        N'›è'
                                              UNION ALL
                                              SELECT    'cu' ,
                                                        N'Óï'
                                              UNION ALL
                                              SELECT    'cuan' ,
                                                        N'Ï‡'
                                              UNION ALL
                                              SELECT    'cui' ,
                                                        N'ƒõ'
                                              UNION ALL
                                              SELECT    'cun' ,
                                                        N'ªv'
                                              UNION ALL
                                              SELECT    'cuo' ,
                                                        N'Âe'
                                              UNION ALL
                                              SELECT    'da' ,
                                                        N'ô\'
                                              UNION ALL
                                              SELECT    'dai' ,
                                                        N'Ï^'
                                              UNION ALL
                                              SELECT    'dan' ,
                                                        N'Ö'
                                              UNION ALL
                                              SELECT    'dang' ,
                                                        N'ÍW'
                                              UNION ALL
                                              SELECT    'dao' ,
                                                        N'ÙÓ'
                                              UNION ALL
                                              SELECT    'de' ,
                                                        N'µƒ'
                                              UNION ALL
                                              SELECT    'den' ,
                                                        N'íY'
                                              UNION ALL
                                              SELECT    'deng' ,
                                                        N'Áã'
                                              UNION ALL
                                              SELECT    'di' ,
                                                        N'œE'
                                              UNION ALL
                                              SELECT    'dia' ,
                                                        N'‡«'
                                              UNION ALL
                                              SELECT    'dian' ,
                                                        N'Úõ'
                                              UNION ALL
                                              SELECT    'diao' ,
                                                        N'ËS'
                                              UNION ALL
                                              SELECT    'die' ,
                                                        N'á√' --±Çá√ 
                                              UNION ALL
                                              SELECT    'ding' ,
                                                        N'Ór'
                                              UNION ALL
                                              SELECT    'diu' ,
                                                        N'‰A'
                                              UNION ALL
                                              SELECT    'dong' ,
                                                        N'Îö'
                                              UNION ALL
                                              SELECT    'dou' ,
                                                        N'Ùa'
                                              UNION ALL
                                              SELECT    'du' ,
                                                        N'Ûº'
                                              UNION ALL
                                              SELECT    'duan' ,
                                                        N'Ö∂' --ªfÖ∂ 
                                              UNION ALL
                                              SELECT    'dui' ,
                                                        N'◊m'
                                              UNION ALL
                                              SELECT    'dun' ,
                                                        N'€v'
                                              UNION ALL
                                              SELECT    'duo' ,
                                                        N'˘z'
                                              UNION ALL
                                              SELECT    'e' ,
                                                        N'˜{'
                                              UNION ALL
                                              SELECT    'en' ,
                                                        N'ﬁÙ'
                                              UNION ALL
                                              SELECT    'eng' ,
                                                        N'ÌE'
                                              UNION ALL
                                              SELECT    'er' ,
                                                        N'òﬁ'
                                              UNION ALL
                                              SELECT    'fa' ,
                                                        N'Ûå'
                                              UNION ALL
                                              SELECT    'fan' ,
                                                        N'û~'
                                              UNION ALL
                                              SELECT    'fang' ,
                                                        N'∑≈'
                                              UNION ALL
                                              SELECT    'fei' ,
                                                        N'Ï]'
                                              UNION ALL
                                              SELECT    'fen' ,
                                                        N'˜a'
                                              UNION ALL
                                              SELECT    'feng' ,
                                                        N'“Ö'
                                              UNION ALL
                                              SELECT    'fo' ,
                                                        N'óÇ'
                                              UNION ALL
                                              SELECT    'fou' ,
                                                        N'¯]'
                                              UNION ALL
                                              SELECT    'fu' ,
                                                        N'™g' --ˆv™g 
                                              UNION ALL
                                              SELECT    'ga' ,
                                                        N'Ùp'
                                              UNION ALL
                                              SELECT    'gai' ,
                                                        N'≠y'
                                              UNION ALL
                                              SELECT    'gan' ,
                                                        N'û∏'
                                              UNION ALL
                                              SELECT    'gang' ,
                                                        N'ëﬂ'
                                              UNION ALL
                                              SELECT    'gao' ,
                                                        N'‰Ü'
                                              UNION ALL
                                              SELECT    'ge' ,
                                                        N'™ò'
                                              UNION ALL
                                              SELECT    'gei' ,
                                                        N'Ωo'
                                              UNION ALL
                                              SELECT    'gen' ,
                                                        N'ìj'
                                              UNION ALL
                                              SELECT    'geng' ,
                                                        N'àÌ' --ÅÉàÌÜØÜ÷ÜÒ 
                                              UNION ALL
                                              SELECT    'gong' ,
                                                        N'É≈' --üÀ⁄CÉ¿É≈ 
                                              UNION ALL
                                              SELECT    'gou' ,
                                                        N'Ÿè'
                                              UNION ALL
                                              SELECT    'gu' ,
                                                        N'Óô'
                                              UNION ALL
                                              SELECT    'gua' ,
                                                        N'‘ü'
                                              UNION ALL
                                              SELECT    'guai' ,
                                                        N'ês'
                                              UNION ALL
                                              SELECT    'guan' ,
                                                        N'˜}'
                                              UNION ALL
                                              SELECT    'guang' ,
                                                        N'ì—'
                                              UNION ALL
                                              SELECT    'gui' ,
                                                        N'˜i'
                                              UNION ALL
                                              SELECT    'gun' ,
                                                        N'÷è'
                                              UNION ALL
                                              SELECT    'guo' ,
                                                        N'ƒB'
                                              UNION ALL
                                              SELECT    'ha' ,
                                                        N'π˛'
                                              UNION ALL
                                              SELECT    'hai' ,
                                                        N'é'
                                              UNION ALL
                                              SELECT    'han' ,
                                                        N'˙['
                                              UNION ALL
                                              SELECT    'hang' ,
                                                        N'„Ï'
                                              UNION ALL
                                              SELECT    'hao' ,
                                                        N'É¡'
                                              UNION ALL
                                              SELECT    'he' ,
                                                        N'Ïg'
                                              UNION ALL
                                              SELECT    'hei' ,
                                                        N'ãœ'
                                              UNION ALL
                                              SELECT    'hen' ,
                                                        N'∫ﬁ'
                                              UNION ALL
                                              SELECT    'heng' ,
                                                        N'à˝' --à˝á÷ 
                                              UNION ALL
                                              SELECT    'hong' ,
                                                        N'Ù\'
                                              UNION ALL
                                              SELECT    'hou' ,
                                                        N'˜c'
                                              UNION ALL
                                              SELECT    'hu' ,
                                                        N'˚I'
                                              UNION ALL
                                              SELECT    'hua' ,
                                                        N'Ãs'
                                              UNION ALL
                                              SELECT    'huai' ,
                                                        N'Ã|'
                                              UNION ALL
                                              SELECT    'huan' ,
                                                        N'ˆd'
                                              UNION ALL
                                              SELECT    'huang' ,
                                                        N'Êw'
                                              UNION ALL
                                              SELECT    'hui' ,
                                                        N'Óú'
                                              UNION ALL
                                              SELECT    'hun' ,
                                                        N'’ü'
                                              UNION ALL
                                              SELECT    'huo' ,
                                                        N'â˛'
                                              UNION ALL
                                              SELECT    'ji' ,
                                                        N'ÛK'
                                              UNION ALL
                                              SELECT    'jia' ,
                                                        N'ÜÌ'
                                              UNION ALL
                                              SELECT    'jian' ,
                                                        N'ËÉ'
                                              UNION ALL
                                              SELECT    'jiang' ,
                                                        N'÷ò'
                                              UNION ALL
                                              SELECT    'jiao' ,
                                                        N'·Ü'
                                              UNION ALL
                                              SELECT    'jie' ,
                                                        N'¿T'
                                              UNION ALL
                                              SELECT    'jin' ,
                                                        N'˝Ñ'
                                              UNION ALL
                                              SELECT    'jing' ,
                                                        N'∏Ñ'
                                              UNION ALL
                                              SELECT    'jiong' ,
                                                        N'ÃW'
                                              UNION ALL
                                              SELECT    'jiu' ,
                                                        N'ô„'
                                              UNION ALL
                                              SELECT    'ju' ,
                                                        N'†Ñ'
                                              UNION ALL
                                              SELECT    'juan' ,
                                                        N'¡\'
                                              UNION ALL
                                              SELECT    'jue' ,
                                                        N'Ëë'
                                              UNION ALL
                                              SELECT    'jun' ,
                                                        N'îh'
                                              UNION ALL
                                              SELECT    'ka' ,
                                                        N'„l'
                                              UNION ALL
                                              SELECT    'kai' ,
                                                        N'Åf' --ÊbÅf 
                                              UNION ALL
                                              SELECT    'kan' ,
                                                        N'≤ô'
                                              UNION ALL
                                              SELECT    'kang' ,
                                                        N'È`'
                                              UNION ALL
                                              SELECT    'kao' ,
                                                        N'ıë'
                                              UNION ALL
                                              SELECT    'ke' ,
                                                        N'ÚS'
                                              UNION ALL
                                              SELECT    'ken' ,
                                                        N'—y'
                                              UNION ALL
                                              SELECT    'keng' ,
                                                        N'ÁH' --é|ÅgÜ{Öûê] 
                                              UNION ALL
                                              SELECT    'kong' ,
                                                        N'èW'
                                              UNION ALL
                                              SELECT    'kou' ,
                                                        N'˙d'
                                              UNION ALL
                                              SELECT    'ku' ,
                                                        N'áø'
                                              UNION ALL
                                              SELECT    'kua' ,
                                                        N'Ûg'
                                              UNION ALL
                                              SELECT    'kuai' ,
                                                        N'˜d'
                                              UNION ALL
                                              SELECT    'kuan' ,
                                                        N'∏U'
                                              UNION ALL
                                              SELECT    'kuang' ,
                                                        N'Ëk'
                                              UNION ALL
                                              SELECT    'kui' ,
                                                        N'Ë^'
                                              UNION ALL
                                              SELECT    'kun' ,
                                                        N'±ó'
                                              UNION ALL
                                              SELECT    'kuo' ,
                                                        N'∑i'
                                              UNION ALL
                                              SELECT    'la' ,
                                                        N'ÌB'
                                              UNION ALL
                                              SELECT    'lai' ,
                                                        N'ª['
                                              UNION ALL
                                              SELECT    'lan' ,
                                                        N'ºh'
                                              UNION ALL
                                              SELECT    'lang' ,
                                                        N'Ü}'
                                              UNION ALL
                                              SELECT    'lao' ,
                                                        N'‹~'
                                              UNION ALL
                                              SELECT    'le' ,
                                                        N'E'
                                              UNION ALL
                                              SELECT    'lei' ,
                                                        N'√ö' --‡œ√ö 
                                              UNION ALL
                                              SELECT    'leng' ,
                                                        N'±ú'
                                              UNION ALL
                                              SELECT    'li' ,
                                                        N'≠Ä'
                                              UNION ALL
                                              SELECT    'lia' ,
                                                        N'Çz'
                                              UNION ALL
                                              SELECT    'lian' ,
                                                        N'¿~'
                                              UNION ALL
                                              SELECT    'liang' ,
                                                        N'Ây'
                                              UNION ALL
                                              SELECT    'liao' ,
                                                        N'≤t'
                                              UNION ALL
                                              SELECT    'lie' ,
                                                        N'˜v'
                                              UNION ALL
                                              SELECT    'lin' ,
                                                        N'ﬁ`' --ﬁ`¡‡ 
                                              UNION ALL
                                              SELECT    'ling' ,
                                                        N'û‚'
                                              UNION ALL
                                              SELECT    'liu' ,
                                                        N'ø©' --ÆFÆMáﬁø© 
                                              UNION ALL
                                              SELECT    'long' ,
                                                        N'⁄L'
                                              UNION ALL
                                              SELECT    'lou' ,
                                                        N'ÁU'
                                              UNION ALL
                                              SELECT    'lu' ,
                                                        N'Î™'
                                              UNION ALL
                                              SELECT    'lv' ,
                                                        N'Ër'
                                              UNION ALL
                                              SELECT    'luan' ,
                                                        N'Åy'
                                              UNION ALL
                                              SELECT    'lue' ,
                                                        N'î^'
                                              UNION ALL
                                              SELECT    'lun' ,
                                                        N'’ì'
                                              UNION ALL
                                              SELECT    'luo' ,
                                                        N'˜w'
                                              UNION ALL
                                              SELECT    'ma' ,
                                                        N'¬Ô'
                                              UNION ALL
                                              SELECT    'mai' ,
                                                        N'ÏA'
                                              UNION ALL
                                              SELECT    'man' ,
                                                        N'Ãp'
                                              UNION ALL
                                              SELECT    'mang' ,
                                                        N'œë'
                                              UNION ALL
                                              SELECT    'mao' ,
                                                        N'Üx'
                                              UNION ALL
                                              SELECT    'me' ,
                                                        N'∞Z' --∞ZÖ– 
                                              UNION ALL
                                              SELECT    'mei' ,
                                                        N'á™'
                                              UNION ALL
                                              SELECT    'men' ,
                                                        N'ÇÉ'
                                              UNION ALL
                                              SELECT    'meng' ,
                                                        N'ÏD' --ÏW€_ 
                                              UNION ALL
                                              SELECT    'mi' ,
                                                        N'¡]'
                                              UNION ALL
                                              SELECT    'mian' ,
                                                        N'¸I'
                                              UNION ALL
                                              SELECT    'miao' ,
                                                        N'èR'
                                              UNION ALL
                                              SELECT    'mie' ,
                                                        N'˜x' --˜x≠ü 
                                              UNION ALL
                                              SELECT    'min' ,
                                                        N'ˆö'
                                              UNION ALL
                                              SELECT    'ming' ,
                                                        N'‘ö'
                                              UNION ALL
                                              SELECT    'miu' ,
                                                        N'÷á'
                                              UNION ALL
                                              SELECT    'mo' ,
                                                        N'ÒÚ' --ÒÚÅi 
                                              UNION ALL
                                              SELECT    'mou' ,
                                                        N'¸E' --¸Eœw 
                                              UNION ALL
                                              SELECT    'mu' ,
                                                        N'îÊ'
                                              UNION ALL
                                              SELECT    'na' ,
                                                        N'Ùõ'
                                              UNION ALL
                                              SELECT    'nai' ,
                                                        N'Âr'
                                              UNION ALL
                                              SELECT    'nan' ,
                                                        N'ãR'
                                              UNION ALL
                                              SELECT    'nang' ,
                                                        N'˝Q'
                                              UNION ALL
                                              SELECT    'nao' ,
                                                        N'ƒû'
                                              UNION ALL
                                              SELECT    'ne' ,
                                                        N'ƒÿ'
                                              UNION ALL
                                              SELECT    'nei' ,
                                                        N'üà' --ƒ€üà 
                                              UNION ALL
                                              SELECT    'nen' ,
                                                        N'ƒ€'
                                              UNION ALL
                                              SELECT    'neng' ,
                                                        N'ƒ‹' --«Ç‡≈‚Ö‰GÜ´ 
                                              UNION ALL
                                              SELECT    'ni' ,
                                                        N'ãÚ'
                                              UNION ALL
                                              SELECT    'nian' ,
                                                        N'≈à'
                                              UNION ALL
                                              SELECT    'niang' ,
                                                        N'·Ñ'
                                              UNION ALL
                                              SELECT    'niao' ,
                                                        N'ÎÂ'
                                              UNION ALL
                                              SELECT    'nie' ,
                                                        N'Ëê'
                                              UNION ALL
                                              SELECT    'nin' ,
                                                        N'íå'
                                              UNION ALL
                                              SELECT    'ning' ,
                                                        N'ùÙ'
                                              UNION ALL
                                              SELECT    'niu' ,
                                                        N'ÏÅ'
                                              UNION ALL
                                              SELECT    'nong' ,
                                                        N'˝P'
                                              UNION ALL
                                              SELECT    'nou' ,
                                                        N'◊k'
                                              UNION ALL
                                              SELECT    'nu' ,
                                                        N'ìx'
                                              UNION ALL
                                              SELECT    'nv' ,
                                                        N'Ù¨'
                                              UNION ALL
                                              SELECT    'nue' ,
                                                        N'Øë'
                                              UNION ALL
                                              SELECT    'nuan' ,
                                                        N'†\' --≥ñ¸Q†\‡G 
                                              UNION ALL
                                              SELECT    'nuo' ,
                                                        N'ñ˛'
                                              UNION ALL
                                              SELECT    'o' ,
                                                        N'ÌM' --öƒâÒÅjÌM 
                                              UNION ALL
                                              SELECT    'ou' ,
                                                        N'ùa'
                                              UNION ALL
                                              SELECT    'pa' ,
                                                        N'–í'
                                              UNION ALL
                                              SELECT    'pai' ,
                                                        N'¥s' --ÊW¥s 
                                              UNION ALL
                                              SELECT    'pan' ,
                                                        N'Ëã'
                                              UNION ALL
                                              SELECT    'pang' ,
                                                        N'≈÷'
                                              UNION ALL
                                              SELECT    'pao' ,
                                                        N'µ^'
                                              UNION ALL
                                              SELECT    'pei' ,
                                                        N'ﬁ\'
                                              UNION ALL
                                              SELECT    'pen' ,
                                                        N'Üœ'
                                              UNION ALL
                                              SELECT    'peng' ,
                                                        N'Ü‘' --õπéáÍCÅnÜ‘ 
                                              UNION ALL
                                              SELECT    'pi' ,
                                                        N'˚G'
                                              UNION ALL
                                              SELECT    'pian' ,
                                                        N'Ú_'
                                              UNION ALL
                                              SELECT    'piao' ,
                                                        N'ëG'
                                              UNION ALL
                                              SELECT    'pie' ,
                                                        N'ã±'
                                              UNION ALL
                                              SELECT    'pin' ,
                                                        N'∆∏'
                                              UNION ALL
                                              SELECT    'ping' ,
                                                        N'ÃO'
                                              UNION ALL
                                              SELECT    'po' ,
                                                        N'∆«'
                                              UNION ALL
                                              SELECT    'pou' ,
                                                        N'ÜR' --ÉÕÜR 
                                              UNION ALL
                                              SELECT    'pu' ,
                                                        N'∆ÿ'
                                              UNION ALL
                                              SELECT    'qi' ,
                                                        N'œÑ'
                                              UNION ALL
                                              SELECT    'qia' ,
                                                        N'˜ƒ'
                                              UNION ALL
                                              SELECT    'qian' ,
                                                        N'øy'
                                              UNION ALL
                                              SELECT    'qiang' ,
                                                        N'≠ô' --¡ÜÉø≠ô 
                                              UNION ALL
                                              SELECT    'qiao' ,
                                                        N'‹N'
                                              UNION ALL
                                              SELECT    'qie' ,
                                                        N'ª]'
                                              UNION ALL
                                              SELECT    'qin' ,
                                                        N'ÃC'
                                              UNION ALL
                                              SELECT    'qing' ,
                                                        N'ôº'
                                              UNION ALL
                                              SELECT    'qiong' ,
                                                        N'≠é'
                                              UNION ALL
                                              SELECT    'qiu' ,
                                                        N'Ù‹'
                                              UNION ALL
                                              SELECT    'qu' ,
                                                        N'”Y'
                                              UNION ALL
                                              SELECT    'quan' ,
                                                        N'ÑÒ'
                                              UNION ALL
                                              SELECT    'que' ,
                                                        N'µ]'
                                              UNION ALL
                                              SELECT    'qun' ,
                                                        N'á›'
                                              UNION ALL
                                              SELECT    'ran' ,
                                                        N'ôL'
                                              UNION ALL
                                              SELECT    'rang' ,
                                                        N'◊å'
                                              UNION ALL
                                              SELECT    'rao' ,
                                                        N'¿@'
                                              UNION ALL
                                              SELECT    're' ,
                                                        N'ü·'
                                              UNION ALL
                                              SELECT    'ren' ,
                                                        N'Ôö'
                                              UNION ALL
                                              SELECT    'reng' ,
                                                        N'Íó'
                                              UNION ALL
                                              SELECT    'ri' ,
                                                        N'Ò_'
                                              UNION ALL
                                              SELECT    'rong' ,
                                                        N'∑\'
                                              UNION ALL
                                              SELECT    'rou' ,
                                                        N'é]'
                                              UNION ALL
                                              SELECT    'ru' ,
                                                        N'îJ'
                                              UNION ALL
                                              SELECT    'ruan' ,
                                                        N'µO'
                                              UNION ALL
                                              SELECT    'rui' ,
                                                        N'â«'
                                              UNION ALL
                                              SELECT    'run' ,
                                                        N'òÙ' --òÙíµ 
                                              UNION ALL
                                              SELECT    'ruo' ,
                                                        N'˙U'
                                              UNION ALL
                                              SELECT    'sa' ,
                                                        N'ñ”' --ô®ñ” 
                                              UNION ALL
                                              SELECT    'sai' ,
                                                        N'ÃÉ' --∫õÃÉ 
                                              UNION ALL
                                              SELECT    'san' ,
                                                        N'Èd'
                                              UNION ALL
                                              SELECT    'sang' ,
                                                        N'Ü '
                                              UNION ALL
                                              SELECT    'sao' ,
                                                        N'ÛÅ'
                                              UNION ALL
                                              SELECT    'se' ,
                                                        N'Ôo' --—S¬{ 
                                              UNION ALL
                                              SELECT    'sen' ,
                                                        N'∫d'
                                              UNION ALL
                                              SELECT    'seng' ,
                                                        N'øL' --È~øL 
                                              UNION ALL
                                              SELECT    'sha' ,
                                                        N'ˆÆ'
                                              UNION ALL
                                              SELECT    'shai' ,
                                                        N'ïÒ'
                                              UNION ALL
                                              SELECT    'shan' ,
                                                        N'˜X'
                                              UNION ALL
                                              SELECT    'shang' ,
                                                        N'æy'
                                              UNION ALL
                                              SELECT    'shao' ,
                                                        N'‰˚'
                                              UNION ALL
                                              SELECT    'she' ,
                                                        N'ô›'
                                              UNION ALL
                                              SELECT    'shen' ,
                                                        N'Øî'
                                              UNION ALL
                                              SELECT    'sheng' ,
                                                        N'Ÿã'
                                              UNION ALL
                                              SELECT    'shi' ,
                                                        N'≠ó' --ˆ|˝aÉæ≠ó 
                                              UNION ALL
                                              SELECT    'shou' ,
                                                        N'Êù'
                                              UNION ALL
                                              SELECT    'shu' ,
                                                        N'Ã†'
                                              UNION ALL
                                              SELECT    'shua' ,
                                                        N'’X'
                                              UNION ALL
                                              SELECT    'shuai' ,
                                                        N'Öi'
                                              UNION ALL
                                              SELECT    'shuan' ,
                                                        N'ƒY'
                                              UNION ALL
                                              SELECT    'shuang' ,
                                                        N'ûì'
                                              UNION ALL
                                              SELECT    'shui' ,
                                                        N'ÀØ'
                                              UNION ALL
                                              SELECT    'shun' ,
                                                        N'ÙB'
                                              UNION ALL
                                              SELECT    'shuo' ,
                                                        N'Ëp'
                                              UNION ALL
                                              SELECT    'si' ,
                                                        N'År' --û[œAÅr 
                                              UNION ALL
                                              SELECT    'song' ,
                                                        N'Êç'
                                              UNION ALL
                                              SELECT    'sou' ,
                                                        N'Øò'
                                              UNION ALL
                                              SELECT    'su' ,
                                                        N'˙â'
                                              UNION ALL
                                              SELECT    'suan' ,
                                                        N'À„'
                                              UNION ALL
                                              SELECT    'sui' ,
                                                        N'Áõ'
                                              UNION ALL
                                              SELECT    'sun' ,
                                                        N'ùñ'
                                              UNION ALL
                                              SELECT    'suo' ,
                                                        N'ŒR'
                                              UNION ALL
                                              SELECT    'ta' ,
                                                        N'“k' --‹c“k 
                                              UNION ALL
                                              SELECT    'tai' ,
                                                        N'†M'
                                              UNION ALL
                                              SELECT    'tan' ,
                                                        N'Ÿy'
                                              UNION ALL
                                              SELECT    'tang' ,
                                                        N'†C'
                                              UNION ALL
                                              SELECT    'tao' ,
                                                        N'Æz' --”ëÆz 
                                              UNION ALL
                                              SELECT    'te' ,
                                                        N'œc'
                                              UNION ALL
                                              SELECT    'teng' ,
                                                        N'ñY' --ÏLÜzñY 
                                              UNION ALL
                                              SELECT    'ti' ,
                                                        N'⁄å'
                                              UNION ALL
                                              SELECT    'tian' ,
                                                        N'≈q'
                                              UNION ALL
                                              SELECT    'tiao' ,
                                                        N'ºg'
                                              UNION ALL
                                              SELECT    'tie' ,
                                                        N'˜—'
                                              UNION ALL
                                              SELECT    'ting' ,
                                                        N'Åh' --ùÏÅh 
                                              UNION ALL
                                              SELECT    'tong' ,
                                                        N'ëq'
                                              UNION ALL
                                              SELECT    'tou' ,
                                                        N'Õ∏'
                                              UNION ALL
                                              SELECT    'tu' ,
                                                        N'˘r'
                                              UNION ALL
                                              SELECT    'tuan' ,
                                                        N'—â'
                                              UNION ALL
                                              SELECT    'tui' ,
                                                        N'ÚD'
                                              UNION ALL
                                              SELECT    'tun' ,
                                                        N'àd'
                                              UNION ALL
                                              SELECT    'tuo' ,
                                                        N'ªX'
                                              UNION ALL
                                              SELECT    'wa' ,
                                                        N'ÌÄ'
                                              UNION ALL
                                              SELECT    'wai' ,
                                                        N'Óì'
                                              UNION ALL
                                              SELECT    'wan' ,
                                                        N'⁄@'
                                              UNION ALL
                                              SELECT    'wang' ,
                                                        N'ñR'
                                              UNION ALL
                                              SELECT    'wei' ,
                                                        N'‹^'
                                              UNION ALL
                                              SELECT    'wen' ,
                                                        N'Ë∑'
                                              UNION ALL
                                              SELECT    'weng' ,
                                                        N'˝N'
                                              UNION ALL
                                              SELECT    'wo' ,
                                                        N'˝}'
                                              UNION ALL
                                              SELECT    'wu' ,
                                                        N'˙F'
                                              UNION ALL
                                              SELECT    'xi' ,
                                                        N'–a'
                                              UNION ALL
                                              SELECT    'xia' ,
                                                        N'Á]'
                                              UNION ALL
                                              SELECT    'xian' ,
                                                        N'˝E'
                                              UNION ALL
                                              SELECT    'xiang' ,
                                                        N'˜P'
                                              UNION ALL
                                              SELECT    'xiao' ,
                                                        N'î√'
                                              UNION ALL
                                              SELECT    'xie' ,
                                                        N'ıÛ'
                                              UNION ALL
                                              SELECT    'xin' ,
                                                        N'·Ö'
                                              UNION ALL
                                              SELECT    'xing' ,
                                                        N'≈B'
                                              UNION ALL
                                              SELECT    'xiong' ,
                                                        N'î∏'
                                              UNION ALL
                                              SELECT    'xiu' ,
                                                        N'˝M'
                                              UNION ALL
                                              SELECT    'xu' ,
                                                        N'ﬁ£'
                                              UNION ALL
                                              SELECT    'xuan' ,
                                                        N'⁄K'
                                              UNION ALL
                                              SELECT    'xue' ,
                                                        N'ûy'
                                              UNION ALL
                                              SELECT    'xun' ,
                                                        N'ËR'
                                              UNION ALL
                                              SELECT    'ya' ,
                                                        N'˝Ö'
                                              UNION ALL
                                              SELECT    'yan' ,
                                                        N'ûπ'
                                              UNION ALL
                                              SELECT    'yang' ,
                                                        N'ò”'
                                              UNION ALL
                                              SELECT    'yao' ,
                                                        N'ËÄ'
                                              UNION ALL
                                              SELECT    'ye' ,
                                                        N'åË' --˚EƒååË 
                                              UNION ALL
                                              SELECT    'yi' ,
                                                        N'˝~'
                                              UNION ALL
                                              SELECT    'yin' ,
                                                        N'ôí'
                                              UNION ALL
                                              SELECT    'ying' ,
                                                        N'◊G'
                                              UNION ALL
                                              SELECT    'yo' ,
                                                        N'Ü—'
                                              UNION ALL
                                              SELECT    'yong' ,
                                                        N'·k'
                                              UNION ALL
                                              SELECT    'you' ,
                                                        N'˜¯'
                                              UNION ALL
                                              SELECT    'yu' ,
                                                        N'†å'
                                              UNION ALL
                                              SELECT    'yuan' ,
                                                        N'Óä'
                                              UNION ALL
                                              SELECT    'yue' ,
                                                        N'˚V'
                                              UNION ALL
                                              SELECT    'yun' ,
                                                        N'Ìç'
                                              UNION ALL
                                              SELECT    'za' ,
                                                        N'Î{'
                                              UNION ALL
                                              SELECT    'zai' ,
                                                        N'øf'
                                              UNION ALL
                                              SELECT    'zan' ,
                                                        N'ï'
                                              UNION ALL
                                              SELECT    'zang' ,
                                                        N'≈K'
                                              UNION ALL
                                              SELECT    'zao' ,
                                                        N'∏^'
                                              UNION ALL
                                              SELECT    'ze' ,
                                                        N'∂è'
                                              UNION ALL
                                              SELECT    'zei' ,
                                                        N'˜e'
                                              UNION ALL
                                              SELECT    'zen' ,
                                                        N'á◊'
                                              UNION ALL
                                              SELECT    'zeng' ,
                                                        N'Ÿõ'
                                              UNION ALL
                                              SELECT    'zha' ,
                                                        N'·m'
                                              UNION ALL
                                              SELECT    'zhai' ,
                                                        N'Ò©'
                                              UNION ALL
                                              SELECT    'zhan' ,
                                                        N'Úñ'
                                              UNION ALL
                                              SELECT    'zhang' ,
                                                        N'≤d'
                                              UNION ALL
                                              SELECT    'zhao' ,
                                                        N'¡^'
                                              UNION ALL
                                              SELECT    'zhe' ,
                                                        N'˙p'
                                              UNION ALL
                                              SELECT    'zhen' ,
                                                        N'¸l'
                                              UNION ALL
                                              SELECT    'zheng' ,
                                                        N'◊C'
                                              UNION ALL
                                              SELECT    'zhi' ,
                                                        N'ÿU'
                                              UNION ALL
                                              SELECT    'zhong' ,
                                                        N'÷A'
                                              UNION ALL
                                              SELECT    'zhou' ,
                                                        N'ÛE'
                                              UNION ALL
                                              SELECT    'zhu' ,
                                                        N'ËT'
                                              UNION ALL
                                              SELECT    'zhua' ,
                                                        N'◊¶'
                                              UNION ALL
                                              SELECT    'zhuai' ,
                                                        N'€J'
                                              UNION ALL
                                              SELECT    'zhuan' ,
                                                        N'ªM'
                                              UNION ALL
                                              SELECT    'zhuang' ,
                                                        N'ëﬁ'
                                              UNION ALL
                                              SELECT    'zhui' ,
                                                        N'ËV'
                                              UNION ALL
                                              SELECT    'zhun' ,
                                                        N'∂õ'
                                              UNION ALL
                                              SELECT    'zhuo' ,
                                                        N'ªm'
                                              UNION ALL
                                              SELECT    'zi' ,
                                                        N'ùn' --ùnÜÄ 
                                              UNION ALL
                                              SELECT    'zong' ,
                                                        N'øv'
                                              UNION ALL
                                              SELECT    'zou' ,
                                                        N'ãÉ'
                                              UNION ALL
                                              SELECT    'zu' ,
                                                        N'÷ä'
                                              UNION ALL
                                              SELECT    'zuan' ,
                                                        N'ﬂ¨'
                                              UNION ALL
                                              SELECT    'zui' ,
                                                        N'ôﬁ'
                                              UNION ALL
                                              SELECT    'zun' ,
                                                        N'„Ü'
                                              UNION ALL
                                              SELECT    'zuo' ,
                                                        N'Ö¯'
                                            ) t
                                    WHERE   word >= @word COLLATE Chinese_PRC_CS_AS_KS_WS
                                    ORDER BY word ASC
                                  )
                             ELSE @word
                        END ) 
                SET @i = @i + 1 
            END 
        RETURN @pinyin 
    END 
GO 

SELECT  dbo.test_GetPinyin('Œ‘≤€£¨Õı¡÷ƒ„∏ˆ2±»«‡ƒÍ') 