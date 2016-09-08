using System;
using System.IO;
using System.Web.Hosting;

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

            return HostingEnvironment.MapPath(virtualPath);
        }

        public static void CreateFolder(string virtualPath)
        {
            var path = HostingEnvironment.MapPath(virtualPath);
            if (Directory.Exists(path)) return;
            if (path != null) Directory.CreateDirectory(path);
        }
    }
}
