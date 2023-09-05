using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

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
    public AzureBlob Image { get; set;}

    public MenuItem()
    {
      Image = new AzureBlob();
    }
  }
}