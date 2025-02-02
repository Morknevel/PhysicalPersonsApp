using Dal.Enums;

namespace Dal.Models.DTO;

public record PhoneNumberDto
{
    public string TelephoneNumber { get; init; }
    
    public PhoneNumberType Type { get; init; }
};