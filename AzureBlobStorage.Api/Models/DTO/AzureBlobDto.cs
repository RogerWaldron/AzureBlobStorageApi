namespace AzureBlobStorage.Api.Models

{
    public class AzureBlobDto
    {
        public string? Uri { get; set; }
        public string? Name { get; set; }
        public string ContentType { get; set; } = null!;
        public Stream Content { get; set; } = null!;
    }  
}