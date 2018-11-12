using System;
using System.IO;
using System.Text;

namespace Common
{
    /// <summary>
    /// 文件操作处理类
    /// </summary>
    public class FileHelper
    {
        /// <summary>
        /// 文件是否存在
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsFileExists(string path)
        {
            return System.IO.File.Exists(path);
        }
        /// <summary>
        /// 从指定文件读取字符串
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string ReadFile(string filePath)
        {
            return System.Text.Encoding.UTF8.GetString(FileHelper.ReadFileForBytes(filePath));
        }
        /// <summary>
        /// 从指定文件中按指定格式读取字符串
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string ReadFile(string filePath, Encoding encode)
        {
            return encode.GetString(FileHelper.ReadFileForBytes(filePath));
        }
        /// <summary>
        /// 从指定文件读取字节
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static byte[] ReadFileForBytes(string filePath)
        {
            return File.ReadAllBytes(filePath); ;
        }
        /// <summary>
        /// 将字符串写入到指定文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileContent"></param>
        /// <returns></returns>
        public static bool WriteFile(string filePath, string fileContent)
        {
            FileHelper.WriteFile(filePath, System.Text.Encoding.UTF8.GetBytes(fileContent));
            return true;
        }
        /// <summary>
        /// 将字符串按指定格式写入到指定文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileContent"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static bool WriteFile(string filePath, string fileContent, Encoding encode)
        {
            FileHelper.WriteFile(filePath, encode.GetBytes(fileContent));
            return true;
        }
        /// <summary>
        /// 将字节写入到指定文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileContent"></param>
        public static void WriteFile(string filePath, byte[] fileContent)
        {
            var dirPath = filePath.Substring(0, filePath.LastIndexOf("\\"));
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            using (var fs = File.Create(filePath))
            {
                fs.Write(fileContent, 0, fileContent.Length);
                fs.Flush();
            }
        }
        /// <summary>
        /// 将字符串追加到指定文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileContent"></param>
        public static void AppendFile(string filePath, string fileContent)
        {
            FileHelper.AppendFile(filePath, System.Text.Encoding.UTF8.GetBytes(fileContent));
        }
        /// <summary>
        /// 将字符串按指定格式追加到指定文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileContent"></param>
        /// <param name="encode"></param>
        public static void AppendFile(string filePath, string fileContent, Encoding encode)
        {
            FileHelper.AppendFile(filePath, encode.GetBytes(fileContent));
        }
        /// <summary>
        /// 将字节追加到指定文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileContent"></param>
        public static void AppendFile(string filePath, byte[] fileContent)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                var dir = filePath.Substring(filePath.LastIndexOf("\\"));
                if (!System.IO.Directory.Exists(dir))
                {
                    System.IO.Directory.CreateDirectory(dir);
                }
            }
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }
            FileInfo info = new FileInfo(filePath);
            using (var fs = info.OpenWrite())
            {
                fs.Write(fileContent, (int)info.Length, fileContent.Length);
                fs.Flush();
            }            
        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool DeleteFile(string filePath)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
            {
                File.Delete(filePath);                
                result = true;
            }
            return result;
        }
        /// <summary>
        /// 文件复制操作
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        /// <param name="overWrite"></param>
        /// <returns></returns>
        public static bool Copy(string source, string dest, bool overWrite)
        {
            bool result = false;
            try
            {
                File.Copy(source, dest, overWrite);
                result = true;
            }
            catch { }
            return result;
        }
        /// <summary>
        /// 文件移动操作
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        /// <returns></returns>
        public static bool Move(string source, string dest)
        {
            bool result = false;
            try
            {
                File.Move(source, dest);
                result = true;
            }
            catch { }
            return result;
        }
        /// <summary>
        /// 加密文件内容
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool Encrypt(string filePath)
        {
            bool result = false;
            try
            {
                string str = FileHelper.ReadFile(filePath);
                str = EncryptionHelper.Base64Helper.Encrypt(str);
                FileHelper.WriteFile(filePath, str);
                result = true;
            }
            catch
            {
            }
            return result;
        }
        /// <summary>
        /// 解密文件内容
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool Decrypt(string filePath)
        {
            bool result = false;
            try
            {
                string str = FileHelper.ReadFile(filePath);
                str = EncryptionHelper.Base64Helper.Decrypt(str);
                FileHelper.WriteFile(filePath, str);
                result = true;
            }
            catch
            {
            }
            return result;
        }

    }
}
