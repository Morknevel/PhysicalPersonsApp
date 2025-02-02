using Dal.Enums;

namespace Dal.Models.DTO;

public record PersonDto{
    
    public int PersonId { get; init; }
    
    public string FirstName { get; init; }
 
    public string LastName { get; init; }
    
    public Gender Gender { get; init; }

    public string IdNumber { get; init; }
    
    public DateTime Birthday { get; init; } 
    
    public int? CityId { get; init; }
    public CityDto? City { get; init; }        

    
    public ICollection<PhoneNumberDto> PhoneNumbers { get; set; } = new List<PhoneNumberDto>();
    
    public string? ImageUrl { get; init; }
    
    public IEnumerable<RelationshipDto> RelatedTo { get; set; } = new List<RelationshipDto>();
    public IEnumerable<RelationshipDto> RelatedFrom { get; set; } = new List<RelationshipDto>();

}