using System;
using System.IO;
//
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;

namespace Common
{
    /// <summary>
    /// 加密解密处理类
    /// </summary>
    public class EncryptionHelper
    {
        #region 对称加密

        /// <summary>
        /// Base64加密、解密
        /// </summary>
        public static class Base64Helper
        {
            /// <summary>
            /// 加密
            /// </summary>
            /// <param name="code"></param>
            /// <returns></returns>
            public static string Encrypt(string code)
            {
                string encode = "";
                byte[] bytes = Encoding.UTF8.GetBytes(code);
                try
                {
                    encode = Convert.ToBase64String(bytes);
                }
                catch
                {
                    encode = code;
                }
                return encode;
            }

            /// <summary>
            /// 解密
            /// </summary>
            /// <param name="code"></param>
            /// <returns></returns>
            public static string Decrypt(string code)
            {
                string decode = "";
                byte[] bytes = Convert.FromBase64String(code);
                try
                {
                    decode = Encoding.UTF8.GetString(bytes);
                }
                catch
                {
                    decode = code;
                }
                return decode;
            }
        }

        /// <summary>
        /// 数据加密算法
        /// </summary>
        public static class DESHelper
        {
            /// <summary>
            /// DES加密
            /// </summary>
            /// <param name="encryptString">待加密的密文</param>
            /// <param name="encryptKey">密匙（8位）</param>
            /// <returns></returns>
            public static string Encrypt(string encryptString, string encryptKey)
            {
                string returnValue;
                try
                {
                    string defaultKey="webBaseFrame";
                    if (string.IsNullOrEmpty(encryptKey))
                    {
                        encryptKey = defaultKey;
                    }
                    if (encryptKey.Length < 6)
                    {
                        encryptKey = encryptKey + defaultKey.Substring(0, 6 - encryptKey.Length);
                    }
                    if (encryptKey.Length > 6)
                    {
                        encryptKey = encryptKey.Substring(0, 6);
                    }
                    byte[] temp = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
                    DESCryptoServiceProvider dES = new DESCryptoServiceProvider();
                    byte[] byteEncrypt = Encoding.UTF8.GetBytes(encryptString);
                    MemoryStream memoryStream = new MemoryStream();
                    CryptoStream cryptoStream = new CryptoStream(memoryStream, dES.CreateEncryptor(Encoding.UTF8.GetBytes(encryptKey), temp), CryptoStreamMode.Write);
                    cryptoStream.Write(byteEncrypt, 0, byteEncrypt.Length);
                    cryptoStream.FlushFinalBlock();
                    returnValue = Convert.ToBase64String(memoryStream.ToArray());

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return returnValue;
            }
            /// <summary>
            /// DES解密
            /// </summary>
            /// <param name="decryptString">密文</param>
            /// <param name="decryptKey">密匙（8位）</param>
            /// <returns></returns>
            public static string Decrypt(string decryptString, string decryptKey)
            {
                string returnValue;
                try
                {
                    string defaultKey = "webBaseFrame";
                    if (string.IsNullOrEmpty(decryptKey))
                    {
                        decryptKey = defaultKey;
                    }
                    if (decryptKey.Length < 6)
                    {
                        decryptKey = decryptKey + defaultKey.Substring(0, 6 - decryptKey.Length);
                    }
                    if (decryptKey.Length > 6)
                    {
                        decryptKey = decryptKey.Substring(0, 6);
                    }
                    byte[] temp = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
                    DESCryptoServiceProvider dES = new DESCryptoServiceProvider();
                    byte[] byteDecryptString = Convert.FromBase64String(decryptString);
                    MemoryStream memoryStream = new MemoryStream();
                    CryptoStream cryptoStream = new CryptoStream(memoryStream, dES.CreateDecryptor(Encoding.UTF8.GetBytes(decryptKey), temp), CryptoStreamMode.Write);

                    cryptoStream.Write(byteDecryptString, 0, byteDecryptString.Length);

                    cryptoStream.FlushFinalBlock();

                    returnValue = Encoding.UTF8.GetString(memoryStream.ToArray());

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return returnValue;

            }

        }

        /// <summary>
        /// DES3重安全加密算法处理类
        /// </summary>
        public static class DES3Helper
        {
            /// <summary>
            /// 3DES 加密
            /// </summary>
            /// <param name="encryptString">待加密密文</param>
            /// <param name="encryptKey1">密匙1(长度必须为8位)</param>
            /// <param name="encryptKey2">密匙2(长度必须为8位)</param>
            /// <param name="encryptKey3">密匙3(长度必须为8位)</param>
            /// <returns></returns>
            public static string Encrypt(string encryptString, string encryptKey1, string encryptKey2, string encryptKey3)
            {

                string returnValue;
                try
                {
                    returnValue = DESHelper.Encrypt(encryptString, encryptKey3);
                    returnValue = DESHelper.Encrypt(returnValue, encryptKey2);
                    returnValue = DESHelper.Encrypt(returnValue, encryptKey1);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return returnValue;

            }
            /// <summary>
            /// 3DES 解密
            /// </summary>
            /// <param name="decryptString">待解密密文</param>
            /// <param name="decryptKey1">密匙1(长度必须为8位)</param>
            /// <param name="decryptKey2">密匙2(长度必须为8位)</param>
            /// <param name="decryptKey3">密匙3(长度必须为8位)</param>
            /// <returns></returns>
            public static string Decrypt(string decryptString, string decryptKey1, string decryptKey2, string decryptKey3)
            {

                string returnValue;
                try
                {
                    returnValue = DESHelper.Decrypt(decryptString, decryptKey1);
                    returnValue = DESHelper.Decrypt(returnValue, decryptKey2);
                    returnValue = DESHelper.Decrypt(returnValue, decryptKey3);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return returnValue;
            }


        }


        /// <summary>   
        /// AES对称加密算法类   
        /// </summary> 
        public class AESHelper
        {
            /// <summary>
            /// AES加密
            /// </summary>
            /// <param name="encryptString">待加密的密文</param>
            /// <param name="encryptKey">加密密匙</param>
            /// <returns></returns>
            public static string Encrypt(string encryptString, string encryptKey)
            {
                string returnValue;
                byte[] temp = Convert.FromBase64String("Rkb4jvUy/ye7Cd7k89QQgQ==");
                Rijndael AESProvider = Rijndael.Create();
                try
                {
                    string defaultKey = "webBaseFramewebB";
                    if (string.IsNullOrEmpty(encryptKey))
                    {
                        encryptKey = defaultKey;
                    }
                    if (encryptKey.Length < 24)
                    {
                        encryptKey = encryptKey + defaultKey.Substring(0, 24 - encryptKey.Length);
                    }
                    if (encryptKey.Length > 24)
                    {
                        encryptKey = encryptKey.Substring(0, 24);
                    }
                    byte[] byteEncryptString = Encoding.UTF8.GetBytes(encryptString);
                    MemoryStream memoryStream = new MemoryStream();
                    CryptoStream cryptoStream = new CryptoStream(memoryStream, AESProvider.CreateEncryptor(Encoding.UTF8.GetBytes(encryptKey), temp), CryptoStreamMode.Write);
                    cryptoStream.Write(byteEncryptString, 0, byteEncryptString.Length);
                    cryptoStream.FlushFinalBlock();
                    returnValue = Convert.ToBase64String(memoryStream.ToArray());
                }

                catch (Exception ex)
                {
                    throw ex;
                }

                return returnValue;

            }
            /// <summary>
            ///AES 解密
            /// </summary>
            /// <param name="decryptString">待解密密文</param>
            /// <param name="decryptKey">解密密钥</param>
            /// <returns></returns>
            public static string Decrypt(string decryptString, string decryptKey)
            {
                string returnValue = "";
                byte[] temp = Convert.FromBase64String("Rkb4jvUy/ye7Cd7k89QQgQ==");
                Rijndael AESProvider = Rijndael.Create();
                try
                {
                    string defaultKey = "webBaseFramewebB";
                    if (string.IsNullOrEmpty(decryptKey))
                    {
                        decryptKey = defaultKey;
                    }
                    if (decryptKey.Length < 24)
                    {
                        decryptKey = decryptKey + defaultKey.Substring(0, 24 - decryptKey.Length);
                    }
                    if (decryptKey.Length > 24)
                    {
                        decryptKey = decryptKey.Substring(0, 24);
                    }
                    byte[] byteDecryptString = Convert.FromBase64String(decryptString);
                    MemoryStream memoryStream = new MemoryStream();
                    CryptoStream cryptoStream = new CryptoStream(memoryStream, AESProvider.CreateDecryptor(Encoding.UTF8.GetBytes(decryptKey), temp), CryptoStreamMode.Write);
                    cryptoStream.Write(byteDecryptString, 0, byteDecryptString.Length);
                    cryptoStream.FlushFinalBlock();
                    returnValue = Encoding.UTF8.GetString(memoryStream.ToArray());
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return returnValue;
            }
        }
        /// <summary>
        /// RC2加密处理类
        /// </summary>
        public static class RC2Helper
        {
            /// <summary>
            /// RC2加密
            /// </summary>
            /// <param name="encryptString">待加密的密文</param>
            /// <param name="encryptKey">密匙(必须为5-16位)</param>
            /// <returns></returns>
            public static string Encrypt(string encryptString, string encryptKey)
            {
                string returnValue;
                try
                {
                    byte[] temp = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
                    RC2CryptoServiceProvider rC2 = new RC2CryptoServiceProvider();
                    byte[] byteEncryptString = Encoding.UTF8.GetBytes(encryptString);
                    MemoryStream memorystream = new MemoryStream();
                    CryptoStream cryptoStream = new CryptoStream(memorystream, rC2.CreateEncryptor(Encoding.UTF8.GetBytes(encryptKey), temp), CryptoStreamMode.Write);
                    cryptoStream.Write(byteEncryptString, 0, byteEncryptString.Length);
                    cryptoStream.FlushFinalBlock();
                    returnValue = Convert.ToBase64String(memorystream.ToArray());

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return returnValue;

            }
            /// <summary>
            /// RC2解密
            /// </summary>
            /// <param name="decryptString">密文</param>
            /// <param name="decryptKey">密匙(必须为5-16位)</param>
            /// <returns></returns>
            public static string Decrypt(string decryptString, string decryptKey)
            {
                string returnValue;
                try
                {
                    byte[] temp = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
                    RC2CryptoServiceProvider rC2 = new RC2CryptoServiceProvider();
                    byte[] byteDecrytString = Convert.FromBase64String(decryptString);
                    MemoryStream memoryStream = new MemoryStream();
                    CryptoStream cryptoStream = new CryptoStream(memoryStream, rC2.CreateDecryptor(Encoding.UTF8.GetBytes(decryptKey), temp), CryptoStreamMode.Write);
                    cryptoStream.Write(byteDecrytString, 0, byteDecrytString.Length);
                    cryptoStream.FlushFinalBlock();
                    returnValue = Encoding.UTF8.GetString(memoryStream.ToArray());
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return returnValue;
            }

        }

        #endregion

        #region 非对称加密

        /// <summary>
        /// MD5加密处理类
        /// </summary>
        public static class MD5Helper
        {
            /// <summary>
            /// 字符串MD5加密
            /// </summary>
            /// <param name="inputedPassword"></param>
            /// <returns></returns>
            public static string Encryt(string inputedPassword)
            {
                return FormsAuthentication.HashPasswordForStoringInConfigFile(inputedPassword, "MD5");
            }
            /// <summary>
            /// 密码比较
            /// </summary>
            /// <param name="inputedPassword"></param>
            /// <param name="currentPassword"></param>
            /// <returns></returns>
            public static bool Verify(string inputedPassword, string currentPassword)
            {
                string encryptedPassword = Encryt(inputedPassword);
                return (encryptedPassword == currentPassword.ToUpper()) ? true : false;
            }
        }

        /// <summary>
        /// SHA1加密处理类
        /// </summary>
        public static class SHA1Helper
        {
            /// <summary>
            /// 字符串SHA1加密
            /// </summary>
            /// <param name="inputedPassword"></param>
            /// <returns></returns>
            public static string Encryt(string inputedPassword)
            {
                return FormsAuthentication.HashPasswordForStoringInConfigFile(inputedPassword, "SHA1");
            }
            /// <summary>
            /// 密码比较
            /// </summary>
            /// <param name="inputedPassword"></param>
            /// <param name="currentPassword"></param>
            /// <returns></returns>
            public static bool Verify(string inputedPassword, string currentPassword)
            {
                string encryptedPassword = Encryt(inputedPassword);
                return (encryptedPassword == currentPassword.ToUpper()) ? true : false;
            }
        }
        #endregion
    }
}
