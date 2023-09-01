using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AzureMeals.Api.Data 
{
  public class ApplicationDbContext : IdentityDbContext
  {
    private readonly DbContextOptions _dbContextOptions;
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
      _dbContextOptions = options;
    }
  }
}