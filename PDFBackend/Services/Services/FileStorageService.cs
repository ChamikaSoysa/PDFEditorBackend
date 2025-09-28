using PDFBackend.Extensions;
using PDFBackend.Interfaces;

namespace PDFBackend.Services.Services
{
    public class FileStorageService: IFileStorageService
    {
        public async Task<string> SaveFileAsync(IFormFile file, string folder = "uploads")
        {

            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty");

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (ext != ".pdf")
                throw new ArgumentException("Only PDF files allowed");
        }

        public async Task<byte[]> ReadFileAsync(string filePath)
        {

        }

        public async Task DeleteFileAsync(string filePath)
        {

        }

    }
}
