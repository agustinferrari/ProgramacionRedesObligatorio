namespace Common.FileUtils.Interfaces
{
    public interface IFileHandler
    {
        bool FileExists(string path);
        string GetFileName(string path);
        long GetFileSize(string path);
    }
}