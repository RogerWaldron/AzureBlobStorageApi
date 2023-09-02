using Microsoft.AspNetCore.Identity;

namespace AzureMeals.Api.Models
{
  public class ApplicationUser : IdentityUser
  {
    public string Name { get; set;} = null!;
  }
}