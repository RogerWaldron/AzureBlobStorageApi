using AzureBlobStorage.Api.Models;
using AzureBlobStorage.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace AzureBlobStorage.Api.Controllers
{
    [Route("api/[controller]")]
  [ApiController]
  public class AzureFileStorageController : ControllerBase
  {
    private readonly IAzureBlobStorage _storage;

    public AzureFileStorageController(IAzureBlobStorage storage)
    {
      _storage = storage;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
      List<AzureBlobDto>? files = await _storage.BlobsGetAllAsync();

      return Ok(files);
    }

    [HttpGet("{filename}")]
    public async Task<IActionResult> Download(string filename)
    {
      
      AzureBlobDto? file = await _storage.BlobDownloadAsync(filename);

      if (file == null)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, $"File {filename} could not be downloaded.");
      } 

      return File(file.Content, file.ContentType, file.Name);
    }

    [HttpDelete("{filename}")]
    public async Task<IActionResult> Delete(string filename)
    {
      AzureBlobResponseDto response = await _storage.BlobDeleteAsync(filename);

      if (response.Error)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, response.Status);
      }

      return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file)
    {
      AzureBlobResponseDto? response = await _storage.BlobUploadAsync(file);

      if (response.Error)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, response.Status);
      }

      return Ok(response);
    }
  }
}