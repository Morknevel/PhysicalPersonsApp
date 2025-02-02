namespace Dal.Models.DTO.CreateDTO;

using System.ComponentModel.DataAnnotations;
using Dal.CustomValidations;
using Dal.Enums;

public class PersonCreateDto
{
    public PersonCreateDto()
    {
        PhoneNumbers = new List<PhoneNumberCreateDto>();
    }

    [Required]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 50 characters.")]
    [RegularExpression(@"^([a-zA-Z]+|[ა-ჰ]+)$",
        ErrorMessage = "Name must contain only Georgian or Latin alphabet letters, not both")]
    public string FirstName { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 50 characters.")]
    [RegularExpression(@"^([a-zA-Z]+|[ა-ჰ]+)$",
        ErrorMessage = "Name must contain only Georgian or Latin alphabet letters, not both")]
    public string LastName { get; set; }

    [Required]
    public Gender Gender { get; set; }

    [Required]
    [RegularExpression(@"^\d{11}$", ErrorMessage = "Number must be 11 digits and contain only numbers.")]
    public string IdNumber { get; set; }

    [Required]
    [MinimumAgeValidation(18)]
    [DataType(DataType.Date)]
    public DateTime Birthday { get; set; }

    public CityCreateDto? CityCreateDto { get; set; }

    public ICollection<PhoneNumberCreateDto>? PhoneNumbers { get; set; }
}