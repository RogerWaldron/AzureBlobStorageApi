using AzureMeals.Api.Models;

namespace AzureMeals.Api.Services
{
  public interface IAzureBlobStorage
  {   
   /// <summary>
   /// Deletes a file with specified name
   /// </summary>
   /// <param name="blobName"></param>
   /// <returns>Blob with status</returns>
    Task<AzureBlobResponseDto> BlobDeleteAsync(string blobName);

    /// <summary>
    /// Downloads a file with specified name
    /// </summary>
    /// <param name="blobName"></param>
    /// <returns>Blob</returns>
    Task<AzureBlobDto> BlobDownloadAsync(string blobName);

    /// <summary>
    /// Returns a list of all files located in the container
    /// </summary>
    /// <returns>Blobs in a list</returns>
    Task<List<AzureBlobDto>> BlobsGetAllAsync();

    /// <summary>
    /// Uploads a file submitted with the request
    /// </summary>
    /// <param name="file"></param>
    /// <returns>Blob with status</returns>
    Task<AzureBlobResponseDto> BlobUploadAsync(IFormFile file);
  }
}
