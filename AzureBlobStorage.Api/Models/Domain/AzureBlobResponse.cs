namespace AzureBlobStorage.Api.Models

{
    public class AzureBlobResponse
    {
        public string? Status { get; set; }
        public bool Error { get; set; }
        public AzureBlob Blob { get; set; }

        public AzureBlobResponse()
        {
            Blob = new AzureBlob();
        }
    }  
}