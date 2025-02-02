using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Dal.CustomValidations;
using Dal.Enums;
using Dal.Models;


public class Person
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int PersonId { get; set; }
    
    [Required]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 50 characters.")]
    [RegularExpression(@"^([a-zA-Z]+|[ა-ჰ]+)$", ErrorMessage = "Name must contain only Georgian or Latin alphabet letters, not both")]
    public string FirstName { get; set; }
    
    [Required]
    [StringLength(50, ErrorMessage = "Name must be between 2 and 50 characters.")]
    [RegularExpression(@"^([a-zA-Z]+|[ა-ჰ]+)$", ErrorMessage = "Name must contain only Georgian or Latin alphabet letters, not both")]
    public string LastName { get; set; }
    
    
    public Gender Gender { get; set; }
    
    [Required]
    [RegularExpression(@"^\d{11}$", ErrorMessage = "Number must be 11 digits and contain only numbers.")]
    public string IdNumber { get; set; }
    
    [Required]
    [MinimumAgeValidation(18)]
    public DateTime Birthday { get; set; } 
    
    
    public int? CityId { get; set; }
    public City? City { get; set; }
    
    public  ICollection<PhoneNumber>? PhoneNumbers { get; set; }
    
    public string? ImageUrl { get; set; }
    
    public IEnumerable<Relationship>? RelatedFrom { get; set; }
    public IEnumerable<Relationship>? RelatedTo { get; set; }
    
}