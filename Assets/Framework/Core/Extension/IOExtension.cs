using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Framework
{
	public static class IOExtension
	{
#if UNITY_EDITOR
        /// <summary>
        /// 打开文件夹
        /// </summary>
        public static void OpenFolder(string folderPath)
        {
#if UNITY_STANDALONE_WIN
            System.Diagnostics.Process.Start("explorer.exe", folderPath);
#elif UNITY_STANDALONE_OSX
            System.Diagnostics.Process.Start("open", path);
#endif
        }
#endif

        /// <summary>
        /// 使路径标准化，去空格并把'\'转换成'/'
        /// </summary>
        public static string MakePathStandard(this string path)
        {
            path = path.Trim().Replace("\\", "/");

            return path;
        }

        /// <summary>
        /// 文件大小等级
        /// </summary>
        public enum FileSizeLevel
        {
            B = 0, KB, MB, GB, TB
        }

        /// <summary>
        /// 获取文件的 带等级的大小
        /// 结果中间有空格
        /// </summary>
        public static string GetFileSize(long length)
        {
            int lev = 0;
            long len = length;
            if (length == 0) return "0 " + ((FileSizeLevel)0).ToString();
            while (len / 1024 > 0)
            {
                len /= 1024;
                lev++;
            }

            return Math.Round(length / Math.Pow(1024, lev), 2).ToString() + " " + ((FileSizeLevel)lev).ToString();
        }

        /// <summary>
        /// 获取文件长度
        /// </summary>
        public static long GetFileLength(this string filePath)
        {
            if (!File.Exists(filePath) && !Directory.Exists(filePath)) return 0;

            return new FileInfo(filePath).Length;
        }

        /// <summary>
        /// 获取文件的 带等级的大小通过文件路径
        /// 结果中间有空格
        /// </summary>
        public static string GetFileSize(this string filePath)
        {
            if (!File.Exists(filePath) && !Directory.Exists(filePath)) return string.Empty;
            FileInfo info = new FileInfo(filePath);

            return GetFileSize(info.Length);
        }

        /// <summary>
        /// 读取 string
        /// </summary>
        public static string ReadText(this string path, Encoding encoding)
        {
            var result = string.Empty;
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs, encoding))
                {
                    result = sr.ReadToEnd();

                    //sr.Close();
                    //fs.Flush();
                    //fs.Close();
                }
            }

            return result;
        }

        /// <summary>
        /// 写入 string
        /// </summary>
        public static void WriteText(this string path, string content, Encoding encoding)
        {
            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs, encoding))
                {
                    sw.Write(content);
                }
            }
        }

        /// <summary>
        /// 读取字节流
        /// </summary>
        public static byte[] ReadBytes(this string path)
        {
            return File.ReadAllBytes(path);
        }

        /// <summary>
        /// 写入字节流
        /// </summary>
        public static void WriteBytes(this string path, byte[] buff)
        {
            File.WriteAllBytes(path, buff);
        }

        /// <summary>
        /// 获取一个路径下的子文件夹
        /// </summary>
        public static string[] GetSubDirs(this string path)
        {
            var di = new DirectoryInfo(path);
            var dirs = di.GetDirectories();
            return dirs.Select(d => d.Name).ToArray();
        }

        /// <summary>
        /// 获取一个路径下子文件
        /// </summary>
        public static List<string> GetSubFiles(this string path, bool isAll = true, string suffix = "")
        {
            List<string> pathList = new List<string>();
            DirectoryInfo info = new DirectoryInfo(path);
            if (!info.Exists)
            {
                return pathList;
            }
            var files = info.GetFiles();
            foreach (var fi in files)
            {
                if (!string.IsNullOrEmpty(suffix))
                {
                    if (!fi.FullName.EndsWith(suffix, System.StringComparison.CurrentCultureIgnoreCase))
                    {
                        continue;
                    }
                }
                pathList.Add(fi.FullName);
            }
            if (isAll)
            {
                var dirs = info.GetDirectories();
                foreach (var d in dirs)
                {
                    pathList.AddRange(GetSubFiles(d.FullName, isAll, suffix));
                }
            }
            return pathList;
        }

        /// <summary>
        /// 是否是一个文件夹
        /// </summary>
        public static bool IsDirectory(this string path)
        {
            FileInfo fi = new FileInfo(path);
            if ((fi.Attributes & FileAttributes.Directory) != 0)
                return true;
            return false;
        }

        /// <summary>
        /// 清空文件夹
        /// </summary>
        public static void ClearDir(this string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
            Directory.CreateDirectory(path);
        }

        /// <summary>
        /// 移除空文件夹
        /// </summary>
        public static bool RemoveEmptyDirectory(this string path)
        {
            if (string.IsNullOrEmpty(path)) throw new Exception("Directory name is invalid.");
            try
            {
                if (!Directory.Exists(path)) return false;
                string[] subDirectoryNames = Directory.GetDirectories(path, "*");
                int subDirectoryCount = subDirectoryNames.Length;
                foreach (string subDirectoryName in subDirectoryNames)
                {
                    if (RemoveEmptyDirectory(subDirectoryName))
                    {
                        subDirectoryCount--;
                    }
                }
                if (subDirectoryCount > 0) return false;
                if (Directory.GetFiles(path, "*").Length > 0) return false;
                Directory.Delete(path);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 拼接路径
        /// </summary>
        public static string CombinePath(this string path, string toCombinePath)
        {
            return Path.Combine(path, toCombinePath).MakePathStandard();
        }

        /// <summary>
        /// 拼接路径
        /// </summary>
        public static string CombinePath(this string path, string[] paths)
        {
            for (int i = 1; i < paths.Length; i++)
            {
                path = path.CombinePath(paths[i]);
            }

            return path.MakePathStandard();
        }

        /// <summary>
        /// 获取文件名
        /// </summary>
        public static string GetFileName(this string path)
        {
            path = path.MakePathStandard();
            var lastIndex = path.LastIndexOf("/");

            return lastIndex >= 0 ? path.Substring(lastIndex + 1) : path;
        }

        /// <summary>
        /// 获取没有后缀的文件名
        /// </summary>
        public static string GetFileNameWithoutExtend(this string path)
        {
            var fileName = GetFileName(path);
            var lastIndex = fileName.LastIndexOf(".");
            return lastIndex >= 0 ? fileName.Substring(0, lastIndex) : fileName;
        }

        /// <summary>
        /// 获取文件后缀
        /// </summary>
        public static string GetFileExtendName(this string path)
        {
            var lastIndex = path.LastIndexOf(".");
            if (lastIndex >= 0)
            {
                return path.Substring(lastIndex);
            }

            return string.Empty;
        }

        /// <summary>
        /// 获取文件夹路径
        /// </summary>
        public static string GetDirPath(this string path)
        {
            path = path.MakePathStandard();
            var lastIndex = path.LastIndexOf("/");

            return path.Substring(0, lastIndex + 1);
        }

        /// <summary>
        /// 获取上一层文件夹名
        /// </summary>
        public static string GetLastDirName(this string path)
        {
            path = path.MakePathStandard();
            var dirs = path.Split('/');
            return dirs[dirs.Length - 2];
        }

        /// <summary>
        /// 转为远程路径
        /// </summary>
        public static string ToRemotePath(this string path)
        {
            path = path.MakePathStandard();

            return path.Contains("://") ? path : ("file:///" + path).Replace("file:////", "file:///");
        }
    }
}