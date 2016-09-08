using System;
using System.IO;

namespace Cinotam.FileManager.Contracts.FileSystemHelpers
{
    public static class FileSystemHelper
    {
        public static void RemoveFile(string absolutePath)
        {
            try
            {
                File.Delete(absolutePath);
            }
            catch (Exception)
            {
                //Ignore
            }
        }

        public static string GetExtension(string fileFileName)
        {
            return Path.GetExtension(fileFileName);
        }

        public static string GetAbsolutePath(string virtualPath)
        {
            var server = HttpContext.Current.Server;
            return server.MapPath(virtualPath);
        }

        public static void CreateFolder(string virtualPath)
        {
            var server = HttpContext.Current.Server;
            var path = server.MapPath(virtualPath);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}
