using System;
using System.IO;
using Common.FileUtils.Interfaces;
using Common.Utils.CustomExceptions;

namespace Common.FileUtils
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
            if (FileExistsAndIsReadable(path))
            {
                return new FileInfo(path).Name;
            }

            throw new InvalidPathException();
        }

        public long GetFileSize(string path)
        {
            if (FileExistsAndIsReadable(path))
            {
                return new FileInfo(path).Length;
            }

            throw new InvalidPathException();
        }
    }
}