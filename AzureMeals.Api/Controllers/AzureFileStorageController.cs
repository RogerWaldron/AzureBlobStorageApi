using AzureMeals.Api.Data;
using AzureMeals.Api.Models;
using AzureMeals.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace AzureMeals.Api.Controllers
{
    [Route("api/[controller]")]
  [ApiController]
  public class AzureFileStorageController : ControllerBase
  {
    private readonly ApplicationDbContext _dbContext;
    private readonly IAzureBlobStorage _storage;

    public AzureFileStorageController(ApplicationDbContext dbContext, IAzureBlobStorage storage)
    {
        _dbContext = dbContext;
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