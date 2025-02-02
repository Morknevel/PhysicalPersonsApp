using System.ComponentModel.DataAnnotations;
using Dal.Enums;

public class PhoneNumber
{
    
    [StringLength(50, MinimumLength = 4, ErrorMessage = "PhoneNumber must be between 4 and 50 characters.")]
    [Key]
    public string TelephoneNumber { get; set; }
    
    public int PersonId { get; set; }
    public Person Person { get; set; }
    
    public PhoneNumberType Type { get; set; }
}   