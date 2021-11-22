namespace CommonProtocol.FileUtils.Interfaces
{
    public interface IFileHandler
    {
        bool FileExistsAndIsReadable(string path);
        string GetFileName(string path);
        long GetFileSize(string path);
        bool IsFilePNG(string path);
        void DeleteFile(string path);
    }
}