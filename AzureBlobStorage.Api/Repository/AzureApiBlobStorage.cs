using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureBlobStorage.Api.Models;
using AzureBlobStorage.Api.Services;

namespace AzureBlobStorage.Api.Repository
{
    public class AzureApiBlobStorage : IAzureBlobStorage
    {
        private readonly string? _storageConnectionString;
        private readonly string _storageContainerName;
        private readonly ILogger<AzureApiBlobStorage> _logger;

        public AzureApiBlobStorage(IConfiguration configuration,
          ILogger<AzureApiBlobStorage> logger)
        {
          _storageConnectionString = configuration.GetValue<string>("StorageConnection");
          _storageContainerName = "file";
          _logger = logger;
        }

        public async Task<AzureBlobResponseDto> BlobDeleteAsync(string blobName)
        {
          BlobContainerClient container = new BlobContainerClient(_storageConnectionString, _storageContainerName);
          BlobClient blob = container.GetBlobClient(blobName);
          
          try {
            await blob.DeleteAsync();
          }
          catch (RequestFailedException ex)
            when (ex.ErrorCode == BlobErrorCode.BlobNotFound) 
          {
            _logger.LogError($"File {blobName} does not exist.");

            return new AzureBlobResponseDto { 
              Error = true,
              Status = $"File {blobName} does not exist."
            };
          }

          return new AzureBlobResponseDto { 
              Status = $"File {blobName} successfully deleted."
            };
        }

        public async Task<AzureBlobDto> BlobDownloadAsync(string blobName)
        {
          BlobContainerClient container = new BlobContainerClient(_storageConnectionString, _storageContainerName);

          try 
          {
            BlobClient blob = container.GetBlobClient(blobName);

            if (await blob.ExistsAsync())
            {
              var data = await blob.OpenReadAsync();
              Stream blobContent = data;

              var content = await blob.DownloadContentAsync();
              var contentType = content.Value.Details.ContentType.ToString();

              return new AzureBlobDto {            
                Uri = blob.Uri.AbsoluteUri.ToString(),
                Name = blobName,
                ContentType = contentType,
                Content = blobContent };
            }
          }
          catch (RequestFailedException ex)
            when(ex.ErrorCode == BlobErrorCode.BlobNotFound)  
          {
            _logger.LogError($"File {blobName} does not exist.");
          }

          return new AzureBlobDto {};
        }

        public async Task<List<AzureBlobDto>> BlobsGetAllAsync()
        {
          BlobContainerClient container = new BlobContainerClient(_storageConnectionString, _storageContainerName);
          await container.CreateIfNotExistsAsync();
          container.SetAccessPolicy(Azure.Storage.Blobs.Models.PublicAccessType.Blob);

          List<AzureBlobDto> files = new ();
          
          await foreach (BlobItem item in container.GetBlobsAsync())
          {
            string uri = container.Uri.AbsoluteUri.ToString();
            var name = item.Name;
            
            files.Add(new AzureBlobDto {
              Uri = uri,
              Name = name,
              ContentType = item.Properties.ContentType
            });
          }

          return files;
        }

        public async Task<AzureBlobResponseDto> BlobUploadAsync(IFormFile file)
        {
          AzureBlobResponseDto response = new();

          BlobContainerClient container = new BlobContainerClient(_storageConnectionString, _storageContainerName);
          await container.CreateIfNotExistsAsync();
          container.SetAccessPolicy(Azure.Storage.Blobs.Models.PublicAccessType.Blob);

          try {
            var fileExtension = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{fileExtension}";
            BlobClient blob = container.GetBlobClient(fileName);
            var headers = new BlobHttpHeaders()
            {
              ContentType = file.ContentType,
            };
            
            await blob.UploadAsync(file.OpenReadStream(), headers);

            response.Status = $"File {file.FileName} uploaded successfully";
            response.Error = false;
            response.Blob.Uri = blob.Uri.AbsoluteUri.ToString();
            response.Blob.Name = blob.Name;
          }
          catch (RequestFailedException ex)
            when (ex.ErrorCode == BlobErrorCode.BlobAlreadyExists)
          {
            _logger.LogError($"File with name {file.FileName} already exists in container.");
            response.Status = $"File with name {file.FileName} already exists, please use a different name.";
            response.Error = true;

            return response;
          }
          catch (RequestFailedException ex)
          {
            _logger.LogError($"Unhandled Exception: ID: {ex.StackTrace} - Message: {ex.Message}");
            response.Status = $"Unexpected Exception: {ex.StackTrace}.";
            response.Error = true;

            return response;
          }
          
          // returns successful object
          return response;
        }
    }
}