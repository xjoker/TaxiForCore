using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Security.AccessControl;

namespace TaxiForCore.FileHelper
{
    public static class FileHelper
    {
        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static string ReadFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using (var sr = new StreamReader(fs, Encoding.UTF8))
                {
                    return sr.ReadToEnd();
                }
            }
            return null;
        }


        /// <summary>
        /// 设定目录完全访问权限
        /// </summary>
        /// <param name="identity">用户实体</param>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public static bool SetDirectoryAccessRule(string identity, string path)
        {
            if (!Directory.Exists(path)) return false;

            const FileSystemRights Rights = FileSystemRights.FullControl;

            //添加访问规则到实际目录
            var AccessRule = new FileSystemAccessRule(identity, Rights,
                InheritanceFlags.None,
                PropagationFlags.NoPropagateInherit,
                AccessControlType.Allow);

            var Info = new DirectoryInfo(path);
            var Security = Info.GetAccessControl(AccessControlSections.Access);

            Security.ModifyAccessRule(AccessControlModification.Set, AccessRule, out bool Result);
            if (!Result) return false;
            //总是允许再目录上进行对象继承
            const InheritanceFlags iFlags = InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit;

            //为继承关系添加访问规则
            AccessRule = new FileSystemAccessRule(identity, Rights,
                iFlags,
                PropagationFlags.InheritOnly,
                AccessControlType.Allow);

            Security.ModifyAccessRule(AccessControlModification.Add, AccessRule, out Result);
            if (!Result) return false;
            Info.SetAccessControl(Security);
            return true;
        }

        /// <summary>
        /// 返回上层路径的文件夹名
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>如果返回失败则为NULL</returns>
        public static string GetParentDirectoryName(string path)
        {
            try
            {
                DirectoryInfo dirInfo = Directory.GetParent(path);
                return Path.GetFileName(dirInfo.FullName);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 返回当前路径上两层的文件夹名
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>如果返回失败则为NULL</returns>
        public static string GetGrampaDirectoryName(string path)
        {
            try
            {
                DirectoryInfo dirInfo = Directory.GetParent(path);
                DirectoryInfo dirInfo1 = Directory.GetParent(dirInfo.FullName);
                return Path.GetFileName(dirInfo1.FullName);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 判断是否为子目录
        /// </summary>
        /// <param name="basePath">需要判断的父路径</param>
        /// <param name="path">被判断的子路径</param>
        /// <returns></returns>
        public static bool IsSubdirectory(string basePath, string path)
        {
            if (basePath == null)
            {
                throw new ArgumentNullException("basePath");
            }
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            basePath = basePath.TrimEnd(new char[]
            {
                Path.DirectorySeparatorChar
            });
            return path.StartsWith(basePath, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 为目录路径结尾增加"/"
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public static string EnsureTrailingSlash(string path)
        {
            return EnsureTrailingCharacter(path, Path.DirectorySeparatorChar);
        }

        /// <summary>
        /// 获取绝对路径
        /// </summary>
        /// <param name="basePath">基础路径</param>
        /// <param name="relativePath">相对路径</param>
        /// <returns></returns>
        public static string GetAbsolutePath(string basePath, string relativePath)
        {
            if (basePath == null)
            {
                throw new ArgumentNullException("basePath");
            }
            if (relativePath == null)
            {
                throw new ArgumentNullException("relativePath");
            }
            Uri uri = new Uri(new Uri(basePath), new Uri(relativePath, UriKind.Relative));
            return uri.LocalPath;
        }

        /// <summary>
        /// 路径尾部增加字符
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="trailingCharacter">添加的字符</param>
        /// <returns></returns>
        public static string EnsureTrailingCharacter(string path, char trailingCharacter)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            if (path.Length == 0 || path[path.Length - 1] == trailingCharacter)
            {
                return path;
            }
            return path + trailingCharacter;
        }

        /// <summary>
        /// 一次性读取文件所有内容
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>返回文件所有内容 MemoryStream </returns>
        public static MemoryStream ReadFileForAll(string filePath)
        {
            if (filePath == null)
            {
                throw new ArgumentNullException("filePath");
            }
            MemoryStream memoryStream = new MemoryStream();
            using (Stream txt = File.OpenRead(filePath))
            {
                txt.CopyTo(memoryStream);
            }
            return memoryStream;
        }

        /// <summary>
        /// 以UTF-8格式写入文件 
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="content">内容</param>
        /// <param name="appends">是否启用追加模式</param>
        public static bool WriteFile(string filePath, string content, bool appends = false)
        {
            return WriteFile(filePath, content, Encoding.UTF8, appends);
        }

        /// <summary>
        /// 写入文件 (自定义编码)
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="content">内容</param>
        /// <param name="encoding">编码 Encoding类型</param>
        /// <param name="appends">是否启用追加模式</param>
        public static bool WriteFile(string filePath, string content, Encoding encoding, bool appends = false)
        {
            try
            {
                long position = 0;
                if (File.Exists(filePath) && appends == false)
                {
                    File.Delete(filePath);
                }
                if (appends && File.Exists(filePath))
                {

                    using (var fs = File.OpenRead(filePath))
                    {
                        position = fs.Length;
                    }
                }
                using (var stream = new FileStream(filePath, FileMode.OpenOrCreate))
                {
                    Encoding encode = encoding;
                    //获得字节数组
                    byte[] data = encode.GetBytes(content);
                    if (appends)
                    {
                        stream.Position = position;
                    }
                    //开始写入
                    stream.Write(data, 0, data.Length);
                    //清空缓冲区、关闭流
                    stream.Flush();
                    return true;
                }
            }
            catch (Exception ex)
            {
                //Log.LogHelper.Instance.LogWrite(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// 搜索路径下特定名称的文件并返回
        /// </summary>
        /// <param name="folderPath">文件夹路径</param>
        /// <param name="fileName">文件名</param>
        /// <param name="se">搜索条件</param>
        /// <returns>string类型的路径数组</returns>
        public static string[] GetFileList(string folderPath, string fileName, SearchOption se)
        {
            string[] files = Directory.GetFiles(folderPath, fileName, se);
            return files;
        }

        /// <summary>
        /// 目录是否存在
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        /// <summary>
        /// 文件是否存在
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool FileExists(string path)
        {
            return File.Exists(path);
        }

        /// <summary>
        /// 打开文件(返回Stream)
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Stream OpenFile(string path)
        {
            return File.OpenRead(path);
        }

        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="path"></param>
        public static bool CreateDirectory(string path)
        {
            string directoryName = Path.GetDirectoryName(path);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
            return Directory.Exists(directoryName);
        }

        /// <summary>
        /// Stream流写入文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="writeToStream">流</param>
        public static void AddFile(string path, Action<Stream> writeToStream)
        {
            if (writeToStream == null)
            {
                throw new ArgumentNullException("writeToStream");
            }
            AddFileCore(path, writeToStream);
        }

        private static void AddFileCore(string path, Action<Stream> writeToStream)
        {
            CreateDirectory(Path.GetDirectoryName(path));
            using (Stream stream = File.Create(path))
            {
                writeToStream(stream);
            }
        }

        /// <summary>
        /// 移动文件，路径需完整并且包含源/目标文件名
        /// </summary>
        /// <param name="source">原路径</param>
        /// <param name="destination">目标路径</param>
        /// <returns></returns>
        public static bool MoveFile(string source, string destination)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (destination == null)
            {
                throw new ArgumentNullException("destination");
            }
            if (string.Equals(source, destination, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            try
            {
                File.Move(source, destination);
                return true;
            }
            catch (IOException)
            {
                return false;
            }
        }


        /// <summary>
        /// 获取一个目录下的所有目录(递归实现)
        /// </summary>
        /// <param name="list">存储列表</param>
        /// <param name="dir">目录路径</param>
        /// <returns></returns>
        public static List<string> GetAllDirectorys(string dir)
        {
            List<string> list = new List<string>();
            string[] dirs = Directory.GetDirectories(dir);
            if (dirs.Length == 0)
            {
                return list;
            }
            foreach (string item in dirs)
            {
                list.Add(item);
                list.AddRange(GetAllDirectorys(item));
            }
            return list;
        }

        /// <summary>
        /// 获取一个目录下的所有文件(递归实现)
        /// </summary>
        /// <param name="list">存储列表</param>
        /// <param name="dir">目录路径</param>
        /// <returns>返回目录下所有文件列表</returns>
        public  static List<string> GetAllFiles(string dir)
        {
            List<string> list = new List<string>();
            if (!Directory.Exists(dir)) return list;
            string[] fileNames = Directory.GetFiles(dir);
            string[] directories = Directory.GetDirectories(dir);
            list.AddRange(fileNames);
            foreach (string item in directories)
            {
                list.AddRange(GetAllFiles(item));
            }
            return list;
        }

        /// <summary>
        /// 文件重命名
        /// </summary>
        /// <param name="sourceFileName"></param>
        /// <param name="newName"></param>
        /// <returns></returns>
        public static bool RenameFile(string sourceFileName, string newName)
        {
            string dir = new FileInfo(sourceFileName).DirectoryName;
            string newpath = Path.Combine(dir, newName);
            return MoveFile(sourceFileName, newpath);
        }

        /// <summary>
        /// 计算文件MD5值
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetFileMD5(string filePath)
        {
            if (File.Exists(filePath))
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                {
                    MD5 md5 = MD5.Create();
                    return BitConverter.ToString(md5.ComputeHash(fs)).Replace("-", "");
                }
            }
            return null;
            
        }

        /// <summary>
        /// 计算文件SHA1值
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetFileSHA1(string filePath)
        {
            if (File.Exists(filePath))
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                {
                    using (BufferedStream bs = new BufferedStream(fs))
                    {
                        var sha1 = SHA1.Create();
                        byte[] hash = sha1.ComputeHash(bs);
                        StringBuilder formatted = new StringBuilder(2 * hash.Length);
                        foreach (byte b in hash)
                        {
                            formatted.AppendFormat("{0:X2}", b);
                        }
                        return formatted.ToString();
                        
                    }
                }
            }
            return null;
        }
    }

}
