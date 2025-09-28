using PDFBackend.DTOs;
using PDFBackend.Extensions;
using PDFBackend.Interfaces;
using System.IO.Compression;

namespace PDFBackend.Services
{
    public class PdfService : IPdfService
    {
        public async Task<byte[]> UpdatePdfTextAsync(string inputPath, Dictionary<int, List<TextEditDto>> edits)
        {
            var doc = new Aspose.Pdf.Document(inputPath);
            foreach (var (pageNum, pageEdits) in edits)
            {
                if (pageNum < 1 || pageNum > doc.Pages.Count) continue;
                var page = doc.Pages[pageNum];
                foreach (var edit in pageEdits)
                {
                    var textFragment = new Aspose.Pdf.Text.TextFragment(Sanitizer.SanitizeString(edit.NewText))
                    {
                        TextState = { FontSize = 12 },
                        Position = new Aspose.Pdf.Text.Position(edit.X, edit.Y)
                    };
                    page.Paragraphs.Add(textFragment);
                }
            }
            using var ms = new MemoryStream();
            doc.Save(ms);
            return ms.ToArray();

        }

        public async Task<byte[]> UpdatePdfMetadataAsync(string inputPath, MetadataDto metadata)
        {
            var doc = new Aspose.Pdf.Document(inputPath);
            doc.Info.Title = Sanitizer.SanitizeString(metadata.Title);
            doc.Info.Author = Sanitizer.SanitizeString(metadata.Author);
            doc.Info.Subject = Sanitizer.SanitizeString(metadata.Subject);
            using var ms = new MemoryStream();
            doc.Save(ms);
            return ms.ToArray();
        }

        public async Task<byte[]> ConvertToDocxAsync(string inputPath)
        {
            var pdfDoc = new Aspose.Pdf.Document(inputPath);
            var doc = new Aspose.Words.Document();
            var builder = new Aspose.Words.DocumentBuilder(doc);

            foreach (Aspose.Pdf.Page page in pdfDoc.Pages)
            {
                var absorber = new Aspose.Pdf.Text.TextAbsorber();
                page.Accept(absorber);
                builder.Writeln(Sanitizer.SanitizeString(absorber.Text));
            }

            using var ms = new MemoryStream();
            doc.Save(ms, Aspose.Words.SaveFormat.Docx);
            return ms.ToArray();
        }

        public async Task<byte[]> ConvertToImagesZipAsync(string inputPath)
        {
     
        }


    }
}
