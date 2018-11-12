using System;
using System.Collections;
using System.IO;

namespace Common
{
    public static class DirHelper
    {
        /// <summary>
        /// 检查文件夹是否存在，不存在则创建文件夹
        /// </summary>
        /// <param name="folderPath"></param>
        public static void CheckFolder(string folderPath)
        {
            if (!System.IO.Directory.Exists(folderPath))
            {
                System.IO.Directory.CreateDirectory(folderPath);
            }
        }
        /// <summary>
        /// 删除指定文件夹
        /// </summary>
        /// <param name="folderPath"></param>
        public static void RemoveFolder(string folderPath)
        {
            if (System.IO.Directory.Exists(folderPath))
            {
                System.IO.Directory.Delete(folderPath, true);
            }
        }

        /// <summary>
        /// 删除指定文件
        /// </summary>
        /// <param name="filepath"></param>
        public static void RemoveFile(string folderPath)
        {
            if (System.IO.File.Exists(folderPath))
            {
                File.Delete(folderPath);
            }
        }
        /// <summary>
        /// 获得所有文件夹
        /// </summary>
        /// <param name="filepath"></param>
        public static ArrayList GetFolders(string folderPath)
        {
            string[] sDirs = System.IO.Directory.GetDirectories(folderPath);
            ArrayList list = new ArrayList();
            DirectoryInfo oDir;
            for (int i = 0; i < sDirs.Length; i++)
            {
                oDir = new DirectoryInfo(sDirs[i]);
                if (oDir.Attributes.ToString().IndexOf("Hidden") == -1)
                {
                    list.Add(sDirs[i].Replace(folderPath, "").Replace("/", "").Replace("\\", ""));
                }
            }
            return list;
        }
        /// <summary>
        /// 获得所有文件
        /// </summary>
        /// <param name="folderpath"></param>
        public static ArrayList GetFiles(string folderPath)
        {
            string[] sDirs = System.IO.Directory.GetFiles(folderPath);
            ArrayList list = new ArrayList();            
            for (int i = 0; i < sDirs.Length; i++)
            {
                FileInfo oDir = new FileInfo(sDirs[i]);
                if (oDir.Attributes.ToString().IndexOf("Hidden") == -1)
                {
                    list.Add(sDirs[i].Replace(folderPath, "").Replace("/", "").Replace("\\", ""));
                }
            }
            return list;
        }

        ///<summary>  
        ///复制文件夹  
        ///</summary>  
        ///<param   name="sourceDirName">源文件夹</param>  
        ///<param   name="destDirName">目标文件夹</param>
        public static void CopyDirectory(string sourceDirName, string destDirName)
        {
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
                File.SetAttributes(destDirName, File.GetAttributes(sourceDirName));
            }
            if (destDirName[destDirName.Length - 1] != Path.DirectorySeparatorChar)
                destDirName = destDirName + Path.DirectorySeparatorChar;
            string[] files = Directory.GetFiles(sourceDirName);
            foreach (string file in files)
            {
                File.Copy(file, destDirName + Path.GetFileName(file), true);
                File.SetAttributes(destDirName + Path.GetFileName(file), FileAttributes.Normal);

            }
            string[] dirs = Directory.GetDirectories(sourceDirName);
            foreach (string dir in dirs)
            {
                CopyDirectory(dir, destDirName + Path.GetFileName(dir));
            }
        }

        /// <summary>
        /// IIS对某个目录是否有操作权限
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool HasOperatePermission(string folderPath)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(folderPath))
            {
                if (Directory.Exists(folderPath))
                {
                    try
                    {
                        var test = folderPath + "test" + new Guid().GetHashCode() + "\\";
                        Directory.CreateDirectory(test);
                        Directory.Delete(test);
                        result = true;
                    }
                    catch { }
                }
            }
            return result;
        }
    }
}
