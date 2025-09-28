namespace PDFBackend.Interfaces
{
    public interface IFileStorageService
    {
        Task<byte[]> ReadFileAsync(string filePath);
        Task<string> SaveFileAsync(IFormFile file, string folder = "uploads");
        Task DeleteFileAsync(string filePath);
    }
}
