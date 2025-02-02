using Dal.Models;
using Dal.Models.DTO.CreateDTO;
using Dal.Repositories;

namespace Dal.Contracts;

public interface IRelationshipRepository : IRepository<Relationship>
{
    Task AddRelationshipAsync(Relationship relationship);
    Task DeleteRelationshipAsync(int relationshipId);
    Task UpdateRelationshipAsync(Relationship relationship);
    Task<IEnumerable<Relationship>> GetPersonRelationshipsAsync(int personId);
}
