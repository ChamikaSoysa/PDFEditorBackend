using PDFBackend.DTOs;

namespace PDFBackend.Interfaces
{
    public interface IPdfService
    {
        Task<byte[]> UpdatePdfTextAsync(string inputPath, Dictionary<int, List<TextEditDto>> edits);

        Task<byte[]> UpdatePdfMetadataAsync(string inputPath, MetadataDto metadata);
    }
}
