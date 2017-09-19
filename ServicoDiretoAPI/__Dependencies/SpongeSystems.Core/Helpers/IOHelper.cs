using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace SpongeSystems.Core.Helpers
{
    public class IOHelper
    {
        public static void CopyFiles(string fromDirectory, string toDirectory, bool deleteFromDiretory)
        {
            if (Directory.Exists(fromDirectory))
            {
                CreateDirectory(toDirectory);
                var dirBase = new DirectoryInfo(fromDirectory);
                foreach (var file in dirBase.GetFiles("*"))
                {
                    File.Move(Path.Combine(fromDirectory, file.Name), Path.Combine(toDirectory, file.Name));

                    if (deleteFromDiretory)
                        File.Delete(Path.Combine(fromDirectory, file.Name));
                }

                foreach (var subDirInfo in dirBase.GetDirectories())
                    CopyFiles(subDirInfo.FullName, Path.Combine(toDirectory, subDirInfo.Name), deleteFromDiretory);

                System.Threading.Thread.Sleep(1000);
                if (deleteFromDiretory)
                    Directory.Delete(fromDirectory);
            }
        }
        public static string GetFileContent(string path)
        {
            if (File.Exists(path))
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    return (sr.ReadToEnd());
                }
            }
            return string.Empty;
        }
        public static void CreateFile(string content, string path)
        {
            CreateDirectory(Path.GetDirectoryName(path));
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.Write(content);
            }
        }
        public static void CreateFile(string content, string path, bool append)
        {
            CreateDirectory(Path.GetDirectoryName(path));
            using (StreamWriter sw = new StreamWriter(path, append))
            {
                sw.Write(content);
            }
        }
        public static void CreateFile(Stream content, string path)
        {
            CreateDirectory(Path.GetDirectoryName(path));
            int length = 256;
            int bytesRead = 0;
            Byte[] buffer = new Byte[length];
            // write the required bytes
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                do
                {
                    bytesRead = content.Read(buffer, 0, length);
                    fs.Write(buffer, 0, bytesRead);
                }
                while (bytesRead == length);
            }
            //content.Dispose();
        }
        public static void CreateFile(byte[] buffer, string path)
        {
            using (MemoryStream stream = new MemoryStream(buffer))
            {
                CreateFile(stream, path);
            }
        }
        public static void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
        public static void DeleteDirectory(string path)
        {
            if (Directory.Exists(path))
                Directory.Delete(path, true);
        }
        public static void DeleteFile(string path)
        {
            if (File.Exists(path))
                File.Delete(path);
        }
        public static void DownloadFile(Uri fromUrl, string localPath)
        {
            System.Net.WebClient client = new WebClient();
            client.DownloadFile(fromUrl, localPath);
        }
        public static Stream DownloadFile(Uri fromUrl)
        {
            try
            {
                using (WebClient wc = new WebClient())
                {
                    byte[] data = wc.DownloadData(fromUrl);
                    return new MemoryStream(data);
                }
            }
            catch (Exception)
            {

                return null;
            }
        }
        public static string GetExtension(Uri url)
        {
            return url.AbsoluteUri.Substring(url.AbsoluteUri.LastIndexOf('.') + 1);
        }
    }
}
