using Dal.Enums;

namespace Dal.Models.DTO.UpdateDTO;

public record PhoneNumberUpdateDto
{
    public string TelephoneNumber { get; init; }
    public PhoneNumberType Type { get; init; }
};