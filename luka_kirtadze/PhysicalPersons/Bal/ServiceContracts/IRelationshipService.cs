using Dal.Models;
using Dal.Models.DTO.CreateDTO;
using Dal.Models.DTO.DeleteDTO;

namespace Bal.ServiceContracts;

public interface IRelationshipService
{
    Task<ApiResponse> AddRelationshipAsync(RelationshipCreateDto relationshipCreateDto);
    Task<ApiResponse> DeleteRelationshipAsync(RelationshipDeleteDto relationshipDeleteDto);
}