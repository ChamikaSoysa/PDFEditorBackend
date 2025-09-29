namespace PDFBackend.DTOs
{
    public class EditMetadataRequest
    {
        public string FilePath { get; set; } = string.Empty;
        public MetadataDto Metadata { get; set; } = new();
    }
}
