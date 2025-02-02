using System.ComponentModel.DataAnnotations;

namespace Dal.Models;

public class City
{
    [Key]
    public int Id { get; set; }
    
    [StringLength(20, MinimumLength = 2, ErrorMessage = "City Name must be between 2 and 50 characters.")]
    public string Name { get; set; }
}