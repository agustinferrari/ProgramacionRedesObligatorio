using System;
using System.IO;
using Common.FileUtils.Interfaces;
using Common.Utils.CustomExceptions;

namespace Common.FileUtils
{
    public class FileHandler : IFileHandler
    {
        public bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public string GetFileName(string path)
        {
            if (FileExists(path))
            {
                return new FileInfo(path).Name;
            }

            throw new InvalidPathException();
        }

        public long GetFileSize(string path)
        {
            if (FileExists(path))
            {
                return new FileInfo(path).Length;
            }

            throw new InvalidPathException();
        }
    }
}