using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Nayelle.Helpers
{
    public static class SystemFile
    {
        private static readonly BaseFile file = new HttpFile();
        public const string Logs = "/Files/Logs/";

        public static string Log(string folder)
        {
            return $"{Logs}{folder}/{DateTime.Now:yyyy-MM}/";
        }

        public static byte[] Download(string key)
        {
            var content = file.Download(key);
            if (content != null)
            {
                content.Position = 0;
                return content.ToArray();
            }
            return null;
        }

        public static void Upload(string key, string content)
        {
            file.Upload(key, content);
        }

        public static void Update(string key, string content)
        {
            file.Update(key, content);
        }

        public static void Update(string key, Stream stream)
        {
            file.Update(key, stream);
        }

        public static void Delete(string key)
        {
            file.Delete(key);
        }

        public static bool Exists(string key)
        {
            return file.Exists(key);
        }

        public static string[] GetFiles(string folder)
        {
            return file.GetFiles(folder);
        }

        public static void CopyFolder(string sourceFolder, string destinationFolder)
        {
            file.CopyFolder(sourceFolder, destinationFolder);
        }

        public static void DeleteFolder(string folder)
        {
            file.DeleteFolder(folder);
        }
    }

    public class IO
    {
        public static string DirectoryIfNone(string virtualPath)
        {
            var sPath = HttpContext.Current.Server.MapPath(virtualPath);
            if (!Directory.Exists(sPath)) Directory.CreateDirectory(sPath);
            return virtualPath;
        }
    }

    public abstract class BaseFile
    {
        public abstract MemoryStream Download(string key);
        public abstract void Upload(string key, string content);
        public abstract void Update(string key, string content);
        public abstract void Update(string key, Stream stream);
        public abstract void Delete(string key);
        public abstract bool Exists(string key);
        public abstract string[] GetFiles(string folder);
        public abstract void CopyFolder(string sourceFolder, string destinationFolder);
        public abstract void DeleteFolder(string folder);
    }

    public class HttpFile : BaseFile
    {
        public override MemoryStream Download(string key)
        {
            try
            {
                MemoryStream memoryStream = new MemoryStream();

                using (FileStream fileStream = new FileStream(HttpContext.Current.Server.MapPath(key), FileMode.Open))
                {
                    fileStream.CopyTo(memoryStream);
                }

                return memoryStream;
            }
            catch
            {
                return null;
            }
        }

        public override void Upload(string key, string content)
        {
            try
            {
                IO.DirectoryIfNone(Path.GetDirectoryName(key));
                var file = HttpContext.Current.Server.MapPath(key);
                using (var sw = new System.IO.StreamWriter(file, true)) sw.WriteLine(content);
            }
            catch
            {
            }
        }

        public override void Update(string key, string content)
        {
            try
            {
                string folder = Path.GetDirectoryName(key);
                IO.DirectoryIfNone(folder);
                var file = HttpContext.Current.Server.MapPath(key);
                using (var sw = new System.IO.StreamWriter(file, true)) sw.WriteLine(content);
            }
            catch
            {
            }
        }

        public override void Update(string key, Stream stream)
        {
            try
            {
                IO.DirectoryIfNone(Path.GetDirectoryName(key));
                var file = HttpContext.Current.Server.MapPath(key);
                using (var fileStream = File.Create(file))
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    stream.CopyTo(fileStream);
                }
            }
            catch
            {
            }
        }

        public override void Delete(string key)
        {
            try
            {
                File.Delete(HttpContext.Current.Server.MapPath(key));
            }
            catch
            {
            }
        }

        public override bool Exists(string key)
        {
            try
            {
                var path = HttpContext.Current.Server.MapPath(key);
                return File.Exists(path) || Directory.Exists(path);
            }
            catch
            {
                return false;
            }
        }

        public override string[] GetFiles(string folder)
        {
            var path = HttpContext.Current.Server.MapPath(folder);
            IO.DirectoryIfNone(Path.GetDirectoryName(folder));
            return Directory.GetFiles(path);
        }

        public override void CopyFolder(string sourceFolder, string destinationFolder)
        {
            var sourcePath = HttpContext.Current.Server.MapPath(sourceFolder);
            var destinationPath = HttpContext.Current.Server.MapPath(destinationFolder);

            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*",
                SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(sourcePath, destinationPath));

            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*",
                SearchOption.AllDirectories))
                File.Copy(newPath, newPath.Replace(sourcePath, destinationPath), true);
        }

        public override void DeleteFolder(string folder)
        {
            try
            {
                Directory.Delete(HttpContext.Current.Server.MapPath(folder), true);
            }
            catch
            {
            }
        }
    }
}