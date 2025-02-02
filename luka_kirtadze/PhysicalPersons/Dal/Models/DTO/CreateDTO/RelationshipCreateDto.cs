using Dal.Enums;

namespace Dal.Models.DTO.CreateDTO;

public record RelationshipCreateDto
{
    public int PersonId { get; init; }
    public int RelatedId { get; init; }
    public RelationshipType  Type { get; init; }
}