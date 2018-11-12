using ICSharpCode.SharpZipLib.Zip;
using System;
using System.IO;

namespace Common
{
    /// <summary>
    /// 压缩解压缩处理类
    /// </summary>
    public class ZIPHelper
    {


        /// <summary>
        /// winRar压缩解压缩处理类
        /// </summary>
        public class WinRarHelper
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            /// <summary>
            /// 初始化
            /// </summary>
            public WinRarHelper()
            {
                process.StartInfo.FileName = "Winrar.exe";
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            }

            #region 压缩
            /// <summary>
            /// 添加文件到压缩文件
            /// </summary>
            /// <param name="pathAndName">文件地址（*.*）</param>
            /// <returns></returns>
            public string Compression(string pathAndName)
            {
                if (string.IsNullOrEmpty(pathAndName)) throw new Exception("被压缩文件地址不能为空!");
                if (pathAndName.IndexOf("-") > -1) throw new Exception("被压缩文件地址不能有符号“-”!");
                var strzipPath = pathAndName;
                try
                {
                    strzipPath = strzipPath.Substring(0, pathAndName.LastIndexOf(".")) + ".zip";
                    if (System.IO.File.Exists(strzipPath)) System.IO.File.Delete(strzipPath);
                    process.StartInfo.Arguments = " a -afzip -ep -o+ -y -ibck " + strzipPath + " " + pathAndName;
                    process.Start();
                    while (!process.HasExited)
                    {
                        System.Threading.Thread.Sleep(100);
                    }
                    GC.Collect();
                }
                catch
                {
                    strzipPath = "";
                }
                return strzipPath;
            }
            /// <summary>
            /// 添加文件到压缩文件
            /// </summary>
            /// <param name="pathAndName">文件地址（*.*）</param>
            /// <param name="strzipPath">输出文件地址（*.*）</param>
            /// <returns></returns>
            public bool Compression(string pathAndName, string strzipPath)
            {
                bool result = false;
                if (string.IsNullOrEmpty(pathAndName)) throw new Exception("被压缩文件地址不能为空!");
                if (pathAndName.IndexOf("-") > -1) throw new Exception("被压缩文件地址不能有符号“-”!");
                if (strzipPath.IndexOf("-") > -1) throw new Exception("输出文件地址不能有符号“-”!");
                if (System.IO.File.Exists(strzipPath)) System.IO.File.Delete(strzipPath);
                process.StartInfo.Arguments = " a -afzip -ep -o+ -y -ibck " + strzipPath + " " + pathAndName;
                process.Start();
                while (!process.HasExited)
                {
                    System.Threading.Thread.Sleep(100);
                }
                GC.Collect();
                result = true;
                return result;
            }
            /// <summary>
            /// 添加文件到压缩文件（指定多个文件压缩）
            /// </summary>
            /// <param name="pathAndName">文件地址（*.*）</param>
            /// <param name="strzipPath">输出文件地址（*.*）</param>
            /// <returns></returns>
            public bool Compression(string[] pathAndName, string strzipPath)
            {
                bool result = false;
                foreach (var item in pathAndName)
                {
                    if (string.IsNullOrEmpty(item)) throw new Exception("被压缩文件地址不能为空!");
                    if (item.IndexOf("-") > -1) throw new Exception("被压缩文件地址不能有符号“-”!");
                }
                if (strzipPath.IndexOf("-") > -1) throw new Exception("输出文件地址不能有符号“-”!");

                if (System.IO.File.Exists(strzipPath)) System.IO.File.Delete(strzipPath);
                string args = " a -afzip -ep -o+ -y -ibck " + strzipPath;
                foreach (var item in pathAndName)
                {
                    args += " " + item;
                }
                process.StartInfo.Arguments = args;
                process.Start();
                while (!process.HasExited)
                {
                    System.Threading.Thread.Sleep(100);
                }
                GC.Collect();
                result = true;
                return result;
            }
            /// <summary>
            /// 压缩并删除原来文件
            /// </summary>
            /// <param name="pathAndName">文件地址</param>
            /// <returns></returns>
            public string CompressionForDelete(string pathAndName)
            {
                if (string.IsNullOrEmpty(pathAndName)) throw new Exception("被压缩文件地址不能为空!");
                if (pathAndName.IndexOf("-") > -1) throw new Exception("被压缩文件地址不能有符号“-”!");
                var strzipPath = pathAndName;
                try
                {
                    if (System.IO.File.Exists(strzipPath)) System.IO.File.Delete(strzipPath);
                    strzipPath = strzipPath.Substring(0, pathAndName.LastIndexOf(".")) + ".zip";
                    process.StartInfo.Arguments = " m -afzip -ep -y -ibck " + strzipPath + " " + pathAndName;
                    process.Start();
                    while (!process.HasExited)
                    {
                        System.Threading.Thread.Sleep(100);
                    }
                    GC.Collect();
                }
                catch
                {
                    strzipPath = "";
                }
                return strzipPath;
            }
            /// <summary>
            /// 压缩并删除原来文件
            /// </summary>
            /// <param name="pathAndName">文件地址</param>
            /// <param name="strzipPath">zip文件地址</param>
            /// <returns></returns>
            public bool CompressionForDelete(string pathAndName, string strzipPath)
            {
                bool result = false;
                if (string.IsNullOrEmpty(pathAndName)) throw new Exception("被压缩文件地址不能为空!");
                if (pathAndName.IndexOf("-") > -1) throw new Exception("被压缩文件地址不能有符号“-”!");

                if (string.IsNullOrEmpty(strzipPath))
                {
                    strzipPath = strzipPath.Substring(0, pathAndName.LastIndexOf(".")) + ".zip";
                }
                if (System.IO.File.Exists(strzipPath)) System.IO.File.Delete(strzipPath);
                process.StartInfo.Arguments = " m -afzip -ep -y -ibck " + strzipPath + " " + pathAndName;
                process.Start();
                while (!process.HasExited)
                {
                    System.Threading.Thread.Sleep(100);
                }
                GC.Collect();
                result = true;
                return result;
            }
            /// <summary>
            /// 压缩文件、目录（若选择删除，则路迳只能是文件）
            /// </summary>
            /// <param name="path">文件、目录地址</param>
            /// <param name="targetPath">指定的输出压缩文件地址</param>
            /// <param name="password"></param>
            /// <param name="isDelete">仅对文件</param>
            /// <returns></returns>
            public bool Compression(string path, string targetPath, string password, bool? isDelete)
            {
                bool result = false;
                if (string.IsNullOrEmpty(path)) throw new Exception("被压缩文件或目录地址不能为空!");
                string args = " a -afzip -ed -y -r -ep1 -ibck ";
                if (isDelete != null && isDelete == true)
                {
                    args = " m -afzip -ed -y -r -ep1 -ibck ";
                }
                if (!string.IsNullOrEmpty(password))
                {
                    args += " -p" + password + " ";
                }
                if (string.IsNullOrEmpty(targetPath))
                {
                    if (path.Substring(path.Length - 1, 1) == @"\")
                    {
                        targetPath = path + (DateTime.Now.Millisecond + (new Random(DateTime.Now.Millisecond).Next(999, 999999))) + ".zip";
                    }
                    else
                    {
                        targetPath = path.Substring(0, path.LastIndexOf(".")) + ".zip";
                    }
                }
                if (System.IO.File.Exists(targetPath)) System.IO.File.Delete(targetPath);
                process.StartInfo.Arguments = args + targetPath + " " + path;
                process.Start();
                while (!process.HasExited)
                {
                    System.Threading.Thread.Sleep(100);
                }
                GC.Collect();
                result = true;
                return result;
            }

            #endregion

            #region 解压缩
            /// <summary>
            /// 解压缩单个文件
            /// </summary>
            /// <param name="pathAndName">zip文件地址</param>
            /// <returns></returns>
            public string Decompression(string pathAndName)
            {
                if (string.IsNullOrEmpty(pathAndName)) throw new Exception("解压缩文件地址不能为空!");
                string targetPath = pathAndName.Substring(0, pathAndName.LastIndexOf("\\") + 1);
                try
                {
                    process.StartInfo.Arguments = " x -o+ -y " + pathAndName + " " + targetPath;
                    process.Start();
                    while (!process.HasExited)
                    {
                        System.Threading.Thread.Sleep(100);
                    }
                    GC.Collect();
                }
                catch
                {
                    targetPath = "";
                }
                return targetPath;
            }

            /// <summary>
            /// 解压缩加密码文件
            /// </summary>
            /// <param name="pathAndName">zip文件地址</param>
            /// <param name="targetPath">解压到指定目录（不指定为当前目录）</param>
            /// <param name="password">可以为空</param>
            /// <returns></returns>
            public bool Decompression(string pathAndName, string targetPath, string password)
            {
                bool result = false;
                try
                {
                    string args = "";
                    if (string.IsNullOrEmpty(pathAndName)) throw new Exception("解压缩文件或目录地址不能为空!");
                    if (string.IsNullOrEmpty(targetPath))
                    {
                        args = " e -o+ -y " + pathAndName;
                    }
                    else
                    {
                        args = " x -o+ -y " + pathAndName + " " + targetPath;
                    }
                    if (!string.IsNullOrEmpty(password))
                    {
                        args += " -p" + password;
                    }
                    else
                    {
                        args += " -p-";
                    }
                    process.StartInfo.Arguments = args;
                    process.Start();
                    while (!process.HasExited)
                    {
                        System.Threading.Thread.Sleep(100);
                    }
                    GC.Collect();
                    result = true;
                }
                catch { }
                return result;
            }
            #endregion
        }


        /// <summary>
        /// ICSharpCode.SharpZipLib
        /// </summary>
        public class SharpZipHelper
        {
            private int compressionLevel = 9;

            private byte[] buffer = new byte[2048];

            /// 默认构造函数
            /// 
            public SharpZipHelper()
            {
                compressionLevel = 9;
                buffer = new byte[2048];
            }
            /// 
            /// 构造函数
            /// 
            /// 缓冲区大小
            /// 压缩率：0-9
            public SharpZipHelper(int bufferSize, int compressionLevel)
            {
                buffer = new byte[bufferSize];
                this.compressionLevel = compressionLevel;
            }


            /// <summary>
            /// 压缩文件
            /// </summary>
            /// <param name="fileToZip">要压缩的文件</param>
            /// <param name="zipedFile">压缩后的文件</param>
            /// <param name="compressionLevel">压缩等级</param>
            /// <param name="blockSize">每次写入大小</param>
            public void ZipFile(string fileToZip, string zipedFile, int _compressionLevel, int _blockSize, string passwords)
            {
                //如果文件没有找到，则报错
                if (!System.IO.File.Exists(fileToZip))
                {
                    throw new System.IO.FileNotFoundException("指定要压缩的文件: " + fileToZip + " 不存在!");
                }
                using (System.IO.FileStream ZipFile = System.IO.File.Create(zipedFile))
                {
                    using (ZipOutputStream ZipStream = new ZipOutputStream(ZipFile))
                    {
                        if (!string.IsNullOrEmpty(passwords))
                        {
                            ZipStream.Password = passwords;
                        }
                        using (System.IO.FileStream StreamToZip = new System.IO.FileStream(fileToZip, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                        {
                            string fileName = fileToZip.Substring(fileToZip.LastIndexOf("\\") + 1);

                            ZipEntry ZipEntry = new ZipEntry(fileName);

                            ZipStream.PutNextEntry(ZipEntry);

                            ZipStream.SetLevel(_compressionLevel);

                            byte[] buffer = new byte[_blockSize];

                            int sizeRead = 0;

                            try
                            {

                                while (true)
                                {
                                    sizeRead = StreamToZip.Read(buffer, 0, buffer.Length);
                                    if (sizeRead > 0)
                                    {
                                        ZipStream.Write(buffer, 0, sizeRead);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                            catch (System.Exception ex)
                            {
                                throw ex;
                            }
                            StreamToZip.Close();
                        }
                        ZipStream.Finish();
                        ZipStream.Close();
                    }
                    ZipFile.Close();
                }

            }
            /// <summary>
            /// 压缩文件
            /// </summary>
            /// <param name="fileToZip"></param>
            /// <param name="zipedFile"></param>
            /// <param name="passwords"></param>
            public void ZipFile(string fileToZip, string zipedFile, string passwords)
            {
                this.ZipFile(fileToZip, zipedFile, compressionLevel, buffer.Length, passwords);
            }

            /// <summary>
            /// 压缩文件
            /// </summary>
            /// <param name="fileToZip">要压缩的文件</param>
            /// <param name="zipedFile">压缩后的文件</param>
            /// <param name="compressionLevel">压缩等级</param>
            /// <param name="blockSize">每次写入大小</param>
            public void ZipFiles(string[] fileToZips, string zipedFile, int _compressionLevel, int _blockSize, string passwords)
            {
                using (System.IO.FileStream ZipFile = System.IO.File.Create(zipedFile))
                {
                    using (ZipOutputStream ZipStream = new ZipOutputStream(ZipFile))
                    {
                        if (!string.IsNullOrEmpty(passwords))
                        {
                            ZipStream.Password = passwords;
                        }
                        foreach (var item in fileToZips)
                        {
                            using (System.IO.FileStream StreamToZip = new System.IO.FileStream(item, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                            {
                                string fileName = item.Substring(item.LastIndexOf("\\") + 1);

                                ZipEntry ZipEntry = new ZipEntry(fileName);

                                ZipStream.PutNextEntry(ZipEntry);

                                ZipStream.SetLevel(_compressionLevel);

                                byte[] buffer = new byte[_blockSize];

                                int sizeRead = 0;

                                try
                                {

                                    while (true)
                                    {
                                        sizeRead = StreamToZip.Read(buffer, 0, buffer.Length);
                                        if (sizeRead > 0)
                                        {
                                            ZipStream.Write(buffer, 0, sizeRead);
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                                catch (System.Exception ex)
                                {
                                    throw ex;
                                }                                
                            }
                        }
                    }
                }

            }
            /// <summary>
            /// 压缩文件
            /// </summary>
            /// <param name="fileToZip"></param>
            /// <param name="zipedFile"></param>
            /// <param name="passwords"></param>
            public void ZipFiles(string[] fileToZips, string zipedFile, string passwords)
            {
                this.ZipFiles(fileToZips, zipedFile, compressionLevel, buffer.Length, passwords);
            }


            /// <summary>
            /// 压缩目录
            /// </summary>
            /// <param name="strDirectory">The directory.</param>
            /// <param name="zipedFile">The ziped file.</param>
            public void ZipDirectory(string strDirectory, string zipedFile, string passwords)
            {
                using (System.IO.FileStream ZipFile = System.IO.File.Create(zipedFile))
                {
                    using (ZipOutputStream s = new ZipOutputStream(ZipFile))
                    {
                        if (!string.IsNullOrEmpty(passwords))
                        {
                            s.Password = passwords;
                        }
                        ZIPStep(strDirectory, s, "", passwords);
                    }
                }
            }

            private static void ZIPStep(string strDirectory, ZipOutputStream s, string parentPath, string passwords)
            {
                if (strDirectory[strDirectory.Length - 1] != Path.DirectorySeparatorChar)
                {
                    strDirectory += Path.DirectorySeparatorChar;
                }

                string[] filenames = Directory.GetFileSystemEntries(strDirectory);

                foreach (string file in filenames)// 遍历所有的文件和目录
                {

                    if (Directory.Exists(file))// 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
                    {
                        string pPath = parentPath;

                        pPath += file.Substring(file.LastIndexOf("\\") + 1);

                        pPath += "\\";

                        ZIPStep(file, s, pPath, passwords);
                    }
                    else // 否则直接压缩文件
                    {
                        //打开压缩文件

                        using (FileStream fs = File.OpenRead(file))
                        {

                            byte[] buffer = new byte[fs.Length];

                            fs.Read(buffer, 0, buffer.Length);

                            string fileName = parentPath + file.Substring(file.LastIndexOf("\\") + 1);

                            ZipEntry entry = new ZipEntry(fileName);

                            entry.DateTime = DateTime.Now;

                            entry.Size = fs.Length;

                            fs.Close();

                            s.PutNextEntry(entry);

                            s.Write(buffer, 0, buffer.Length);
                        }

                    }
                }
            }





            /// <summary>
            /// 解压缩一个 zip 文件。
            /// </summary>
            /// <param name="zipedFile">The ziped file.</param>
            /// <param name="strDirectory">The STR directory.</param>
            /// <param name="password">zip 文件的密码。</param>
            /// <param name="overWrite">是否覆盖已存在的文件。</param>
            public void UnZipFile(string zipedFile, string strDirectory, string password, bool overWrite)
            {

                if (strDirectory == "")
                    strDirectory = Directory.GetCurrentDirectory();
                if (!strDirectory.EndsWith("\\"))
                    strDirectory = strDirectory + "\\";

                using (ZipInputStream s = new ZipInputStream(File.OpenRead(zipedFile)))
                {
                    s.Password = password;
                    ZipEntry theEntry;

                    while ((theEntry = s.GetNextEntry()) != null)
                    {
                        string directoryName = "";
                        string pathToZip = "";
                        pathToZip = theEntry.Name;
                        if (pathToZip != "")
                            directoryName = Path.GetDirectoryName(pathToZip) + "\\";

                        string fileName = Path.GetFileName(pathToZip);

                        Directory.CreateDirectory(strDirectory + directoryName);

                        if (fileName != "")
                        {
                            if ((File.Exists(strDirectory + directoryName + fileName) && overWrite) || (!File.Exists(strDirectory + directoryName + fileName)))
                            {
                                using (FileStream streamWriter = File.Create(strDirectory + directoryName + fileName))
                                {
                                    int size = 2048;

                                    byte[] data = new byte[2048];

                                    while (true)
                                    {
                                        size = s.Read(data, 0, data.Length);
                                        if (size > 0)
                                            streamWriter.Write(data, 0, size);
                                        else
                                            break;
                                    }
                                    streamWriter.Close();
                                }
                            }
                        }
                    }
                    s.Close();
                }
            }
            //end
        }
    }
}
