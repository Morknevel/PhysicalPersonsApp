using Dal.Enums;

namespace Dal.Models.DTO.CreateDTO;

public class PhoneNumberCreateDto
{
    public string TelephoneNumber { get; set; }
    public PhoneNumberType Type { get; init; }
}