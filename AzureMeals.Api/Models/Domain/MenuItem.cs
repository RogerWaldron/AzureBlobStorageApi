using System.ComponentModel.DataAnnotations;

namespace AzureMeals.Api.Models
{
  public class MenuItem
  {
    [Key]
    public int Id { get; set;}
    public required string Name { get; set;}
    public required string Description { get; set;}
    [Range(1, int.MaxValue)]
    public double Price { get; set;}
    public string Category { get; set;} = "";
    public string Tag { get; set;} = "";
    public string Image { get; set;} = "";
  }
}