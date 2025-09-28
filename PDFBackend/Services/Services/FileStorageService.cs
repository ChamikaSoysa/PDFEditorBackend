using PDFBackend.Extensions;
using PDFBackend.Interfaces;

namespace PDFBackend.Services.Services
{
    public class FileStorageService: IFileStorageService
    {
        private readonly string _baseTempPath = Path.Combine(Path.GetTempPath(), "PdfBackend");
        public async Task<string> SaveFileAsync(IFormFile file, string folder = "uploads")
        {

            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty");

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (ext != ".pdf")
                throw new ArgumentException("Only PDF files allowed");

            var safeName = Sanitizer.SanitizeFileName(Path.GetFileNameWithoutExtension(file.FileName)) + ext;
            var dir = Path.Combine(_baseTempPath, folder, Guid.NewGuid().ToString());
            Directory.CreateDirectory(dir);

            var fullPath = Path.Combine(dir, safeName);
            using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);
            return fullPath;

        }

        public async Task<byte[]> ReadFileAsync(string filePath)
        {
            if (!File.Exists(filePath)) throw new FileNotFoundException();
            return await File.ReadAllBytesAsync(filePath);
        }

        public async Task DeleteFileAsync(string filePath)
        {

        }

    }
}
