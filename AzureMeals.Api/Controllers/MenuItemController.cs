using System.Linq.Expressions;
using System.Net;
using AzureMeals.Api.Data;
using AzureMeals.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

namespace AzureMeals.Api.Controllers
{
    [Route("api/[controller]")]
  [ApiController]
  public class MenuItemController : ControllerBase
  {
    private readonly ApplicationDbContext _dbContext;
    private ApiResponse _response;

    public MenuItemController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        _response = new ApiResponse();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
      try {
        var results = await _dbContext.MenuItems.ToListAsync();
        if (results == null) {
          _response.IsSuccess = false;
          return BadRequest(_response);
        }

        _response.Data = results;
        _response.StatusCode = HttpStatusCode.OK;
      }
      catch (Exception ex) {
        _response.IsSuccess = false;
        _response.ErrorMessages.Append(Convert.ToString(ex.Message));
      }
      
      return Ok(_response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
      if (id == 0) {
        _response.IsSuccess = false;
        _response.StatusCode = HttpStatusCode.BadRequest;

        return BadRequest(_response);
      }

      MenuItem? menuItem = await _dbContext.MenuItems.FirstOrDefaultAsync(item => item.Id == id );
      
      if (menuItem == null) {
        _response.IsSuccess = false;
        _response.StatusCode = HttpStatusCode.NotFound;

        return NotFound(_response);
      }

      _response.Data = menuItem;
      _response.StatusCode = HttpStatusCode.OK;

      return Ok(_response);
    }
  }

  // [HttpPost]
  // public async Task<ActionResult<ApiResponse>> CreateMenuItem([FromForm]MenuItem newMenuItem)
  // {
  //   try {
  //     if (ModelState.IsValid)
  //     {

  //     }
  //   }
  //   catch (Exception ex)
  //   {
  //     _response.IsSuccess = false;
  //     _res _response.ErrorMessages.Append(Convert.)
  //   } 

  //   return _response;
  // }
}