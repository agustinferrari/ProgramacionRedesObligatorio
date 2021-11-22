using System;
using System.IO;
using CommonProtocol.Utils.CustomExceptions;
using CommonProtocol.FileUtils.Interfaces;

namespace CommonProtocol.FileUtils
{
    public class FileHandler : IFileHandler
    {
        public bool FileExistsAndIsReadable(string path)
        {
            bool readable = File.Exists(path);
            if (readable)
                try
                {
                    File.OpenRead(path).Close();
                }
                catch (UnauthorizedAccessException)
                {
                    readable = false;
                }
            return readable;
        }

        public string GetFileName(string path)
        {
            string fileName = "";
            if (FileExistsAndIsReadable(path))
                fileName = new FileInfo(path).Name;
            return fileName;
        }

        public long GetFileSize(string path)
        {
            long fileSize = 0;
            if (FileExistsAndIsReadable(path))
                fileSize = new FileInfo(path).Length;
            return fileSize;
        }

        public bool IsFilePNG(string path)
        {
            if (FileExistsAndIsReadable(path))
                return new FileInfo(path).Extension.ToLower() == ".png";
            return false;
        }

        public void DeleteFile(string path)
        {
            if (FileExistsAndIsReadable(path))
                File.Delete(path);
        }
    }
}