
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace AzureMeals.Api.Services
{
    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient _blobClient;

        public BlobService(BlobServiceClient blobClient)
        {
            _blobClient = blobClient;
        }

        public async Task<bool> DeleteBlob(string blobName, string containerName)
        {
          BlobContainerClient container = _blobClient.GetBlobContainerClient(containerName);
          BlobClient blob = container.GetBlobClient(blobName);
          
          return await blob.DeleteIfExistsAsync();
        }

        public async Task<string> GetBlob(string blobName, string containerName)
        {
          BlobContainerClient container = _blobClient.GetBlobContainerClient(containerName);
          BlobClient blob = container.GetBlobClient(blobName);

          return blob.Uri.AbsoluteUri;
        }

        public async Task<string> UploadBlob(string blobName, string containerName, IFormFile file)
        {

          BlobContainerClient container = _blobClient.GetBlobContainerClient(containerName);
          BlobClient blob = container.GetBlobClient(blobName);
          var headers = new BlobHttpHeaders()
          {
            ContentType = file.ContentType,
          };
          
          var result = await blob.UploadAsync(file.OpenReadStream(), headers);
          if (result != null)
          {
            return await GetBlob(blobName, containerName);
          }

          return "";
        }
    }
}