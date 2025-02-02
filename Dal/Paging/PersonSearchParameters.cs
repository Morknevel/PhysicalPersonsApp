using Dal.Enums;

namespace Dal.Paging;

public class PersonSearchParameters
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public Gender? Gender { get; set; }
    public string? IdNumber { get; set; }
    public DateTime? StartBirthDate { get; set; }
    public DateTime? EndBirthDate { get; set; }
    public int? CityId { get; set; }
}
