using Dal.Enums;

namespace Dal.Models;

public class Relationship
{
    public int PersonId { get; set; }
    public Person Person { get; set; }

    public int RelatedId { get; set; }
    public Person RelatedPerson { get; set; }
    
    public RelationshipType RelationType { get; set; } 
    
}