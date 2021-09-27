namespace Common.FileUtils.Interfaces
{
    public interface IFileHandler
    {
        bool FileExistsAndIsReadable(string path);
        string GetFileName(string path);
        long GetFileSize(string path);
    }
}