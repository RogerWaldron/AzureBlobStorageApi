namespace AzureBlobStorage.Api.Models

{
    public class AzureBlobResponseDto
    {
        public string? Status { get; set; }
        public bool Error { get; set; } = false;
        public AzureBlobDto Blob { get; set; }

        public AzureBlobResponseDto()
        {
            Blob = new AzureBlobDto();
        }
    }  
}