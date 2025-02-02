using Dal.Enums;

namespace Dal.Models.DTO;

 public record RelationshipDto
{
    public int PersonId { get; init; }
    public int RelatedId { get; init; }
    public RelationshipType RelationType { get; init; }  
}