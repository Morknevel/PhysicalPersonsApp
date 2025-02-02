using AutoMapper;
using Bal.ServiceContracts;
using Dal.Contracts;
using Dal.Models;
using Dal.Models.DTO.CreateDTO;
using Dal.Models.DTO.DeleteDTO;

namespace Bal.Services;

public class RelationshipService : IRelationshipService
{
    private readonly ApiResponse _response;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RelationshipService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _response = new();
    }
    public async Task<ApiResponse> AddRelationshipAsync(RelationshipCreateDto relationshipCreateDto)
    {
        var person = await _unitOfWork.Persons.GetPersonByIdAsync(relationshipCreateDto.PersonId);
        if (person == null)
        {
            _response.IsSuccess = false;
            _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
            _response.ErrorMessages.Add("Person not found");
            return _response;
        }
        
        var relatedPerson = await _unitOfWork.Persons.GetPersonByIdAsync(relationshipCreateDto.RelatedId);
        if (relatedPerson == null)
        {
            _response.IsSuccess = false;
            _response.StatusCode = System.Net.HttpStatusCode.NotFound;
            _response.ErrorMessages.Add("Related person not found.");
            return _response; 
        }
        if (relationshipCreateDto.PersonId == relationshipCreateDto.RelatedId)
        {
            _response.IsSuccess = false;
            _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
            _response.ErrorMessages.Add("Cannot create a relationship to oneself.");
            return _response;
        }
        
        var relationship = _mapper.Map<Relationship>(relationshipCreateDto);

        await _unitOfWork.Relationships.AddRelationshipAsync(relationship);
        await _unitOfWork.SaveAsync();
        _response.IsSuccess = true;
        _response.StatusCode = System.Net.HttpStatusCode.Created;
        _response.Result = relationship;
        return _response;
    }


    public async Task<ApiResponse> DeleteRelationshipAsync(RelationshipDeleteDto relationshipDeleteDto)
    {
        var relationship = await _unitOfWork.Relationships
            .GetAsync(r => (r.PersonId == relationshipDeleteDto.PersonId && r.RelatedId == relationshipDeleteDto.RelatedId) ||
                                      (r.PersonId == relationshipDeleteDto.RelatedId && r.RelatedId == relationshipDeleteDto.PersonId));
        if (relationship == null)
        {
            _response.IsSuccess = false;
            _response.StatusCode = System.Net.HttpStatusCode.NotFound;
            _response.ErrorMessages.Add("Relationship not found");
        }

        _unitOfWork.Relationships.Remove(relationship);
        await _unitOfWork.SaveAsync();
        _response.IsSuccess = true;
        _response.StatusCode = System.Net.HttpStatusCode.NoContent;
        return _response;
    }
}