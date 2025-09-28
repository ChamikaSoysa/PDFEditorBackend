using PDFBackend.Extensions;
using PDFBackend.Interfaces;

namespace PDFBackend.Services.Services
{
    public class FileStorageService: IFileStorageService
    {
        public async Task<string> SaveFileAsync(IFormFile file, string folder = "uploads")
        {

        }

        public async Task<byte[]> ReadFileAsync(string filePath)
        {

        }

        public async Task DeleteFileAsync(string filePath)
        {

        }

    }
}
