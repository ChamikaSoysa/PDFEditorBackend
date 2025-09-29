namespace PDFBackend.DTOs
{
    public class EditTextRequest
    {
        public string FilePath { get; set; } = string.Empty;
        public Dictionary<int, List<TextEditDto>> Edits { get; set; } = new();
    }
}
