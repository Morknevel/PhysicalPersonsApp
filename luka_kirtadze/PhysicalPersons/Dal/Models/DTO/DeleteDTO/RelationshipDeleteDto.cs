namespace Dal.Models.DTO.DeleteDTO;

public record RelationshipDeleteDto
{
   public int PersonId { get; init; }
   public int RelatedId { get; init; }
}
   