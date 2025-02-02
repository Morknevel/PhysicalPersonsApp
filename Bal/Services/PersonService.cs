using System.Net;
using AutoMapper;
using Bal.Helpers;
using Bal.Infrastracture;
using Bal.ServiceContracts;
using Dal.Contracts;
using Dal.Models;
using Dal.Models.DTO;
using Dal.Models.DTO.CreateDTO;
using Dal.Models.DTO.UpdateDTO;
using Dal.Paging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;


namespace Bal.Services;

public class PersonService : IPersonService
{
    private readonly ApiResponse _response;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRelationshipRepository _relationshipRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<PersonService> _logger;
    private readonly IFileFactory _fileFactory;
    private readonly IFileService _fileService;
    

    public PersonService(IUnitOfWork unitOfWork, IMapper mapper, IRelationshipRepository relationshipRepository,
        ILogger<PersonService> logger, IFileFactory fileFactory, IFileService fileService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _relationshipRepository = relationshipRepository;
        _logger = logger;
        _fileFactory = fileFactory;
        _fileService = fileService;
        _response = new();
    }


    public async Task<ApiResponse> AddPersonAsync(PersonCreateDto personCreateDto)
    {
        if (await _unitOfWork.Persons.AnyAsync(p => p.IdNumber == personCreateDto.IdNumber))
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.ErrorMessages = ["A person with the given ID number already exists."];
            return _response;
        }

        int? cityId = null;

        if (!string.IsNullOrWhiteSpace(personCreateDto.CityCreateDto?.Name))
        {
            var city = await _unitOfWork.Cities.GetAsync(c => c.Name == personCreateDto.CityCreateDto.Name);
            if (city == null)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("City does not exist. Please add the city first.");
                return _response;
            }

            cityId = city.Id;
        }

        foreach (var phoneNumberDto in personCreateDto.PhoneNumbers)
        {
            bool phoneExists =
                await _unitOfWork.PhoneNumbers.AnyAsync(pn => pn.TelephoneNumber == phoneNumberDto.TelephoneNumber);
            if (phoneExists)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("Duplicate phone number: " + phoneNumberDto.TelephoneNumber);
                return _response;
            }
        }

        try
        {
            var person = _mapper.Map<Person>(personCreateDto);
            person.CityId = cityId;
            await _unitOfWork.Persons.CreatePerson(person);
            _response.Result = _mapper.Map<PersonDto>(person);
            await _unitOfWork.SaveAsync();
            _response.StatusCode = HttpStatusCode.Created;
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.InternalServerError;
            _response.ErrorMessages = new List<string> { ex.Message };
        }

        return _response;
    }

    public async Task<ApiResponse> UpdatePersonAsync(int id, PersonUpdateDto personUpdateDto)
    {
        var person = await _unitOfWork.Persons.GetPersonByIdAsync(id);
        if (person == null)
        {
            _response.IsSuccess = false;
            _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
            _response.ErrorMessages.Add("Person not found");
            return _response;
        }

        _mapper.Map(personUpdateDto, person);

        if (!string.IsNullOrWhiteSpace(personUpdateDto.CityUpdateDto?.Name))
        {
            var city = await _unitOfWork.Cities.GetAsync(c => c.Name == personUpdateDto.CityUpdateDto.Name);
            if (city == null)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("City does not exist. Please add the city first.");
                return _response;
            }

            person.CityId = city.Id;
        }

        try
        {
            _unitOfWork.Persons.UpdatePerson(person);
            await _unitOfWork.SaveAsync();
            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            return _response;
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.InternalServerError;
            _response.ErrorMessages = new List<string> { ex.Message };
            return _response;
        }
    }


    public async Task<ApiResponse> DeletePersonAsync(int personId)
    {
        try
        {
            var relationships = await _unitOfWork.Relationships
                .FindAsync(r => r.PersonId == personId || r.RelatedId == personId);
            foreach (var relationship in relationships)
            {
                _unitOfWork.Relationships.RemoveRange(relationships);
            }

            await _unitOfWork.Persons.RemovePersonAsync(personId);
            await _unitOfWork.SaveAsync();
            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            return _response;
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string> { ex.Message };
            return _response;
        }
    }

    public async Task<ApiResponse> UpdatePersonPhotoAsync(int personId, IFormFile image)
    {
        var person = await _unitOfWork.Persons.GetPersonByIdAsync(personId);
        if (person == null)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.ErrorMessages.Add("Person not found");
            return _response;
        }
        IFile file = _fileFactory.CreateFile(image);
        var fileResult = _fileService.UploadImage(file);
        if (!fileResult.IsSuccess)
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.ErrorMessages.Add("Person not found");
            return _response;
        }
        await _unitOfWork.SaveAsync();
        return _response;
    }

    public async Task<ApiResponse> DeletePersonPhotoAsync(int personId)
    {
        var person = await _unitOfWork.Persons.GetPersonByIdAsync(personId);
        if (person == null)
        {
            _response.IsSuccess = false;
            _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
            _response.ErrorMessages.Add("Person not found");
            return _response;
        }

        if (person.ImageUrl != null)
        {
            person.ImageUrl = null;
            _unitOfWork.Persons.UpdatePerson(person);
        }

        await _unitOfWork.SaveAsync();
        return _response;
    }

    public async Task<ApiResponse> QuickSearchAsync(string searchTerm)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = ["Search term cannot be empty"];
                _response.StatusCode = HttpStatusCode.BadRequest;
            }

            var people = await _unitOfWork.Persons.QuickSearchAsync(searchTerm);
            _response.ErrorMessages = ["Search completed successfully"];
            _response.StatusCode = HttpStatusCode.OK;
            _response.Result = people;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error performing quick search with term: {SearchTerm}", searchTerm);
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string> { ex.Message };
            _response.StatusCode = HttpStatusCode.BadRequest;
        }

        return _response;
    }

    public async Task<ApiResponse> DetailedSearchAsync(PersonSearchParameters parameters, int pageNumber = 1,
        int pageSize = 10)
    {
        try
        {
            if (parameters == null)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = ["Search term cannot be empty"];
                _response.StatusCode = HttpStatusCode.BadRequest;
            }

            var (people, totalCount) = await _unitOfWork.Persons.DetailedSearchAsync(parameters, pageNumber, pageSize);

            var pagedResponse = new PagedResponse<Person>
            {
                Data = people,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };
            _response.StatusCode = HttpStatusCode.OK;
            _response.Result = pagedResponse;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error performing detailed search with parameters: {@Parameters}", parameters);
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string> { ex.Message };
            _response.StatusCode = HttpStatusCode.BadRequest;
        }

        return _response;
    }

    public async Task<ApiResponse> GetPersonDetailsAsync(int personId)
    {
        var person = await _unitOfWork.Persons.GetWithDetailsAsync(personId);
        if (person == null)
        {
            _response.IsSuccess = false;
            _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
            _response.ErrorMessages.Add("Person not found");
            return _response;
        }

        var relationships = await _relationshipRepository.GetPersonRelationshipsAsync(personId);

        person.RelatedTo = relationships.Where(r => r.PersonId == personId).ToList();
        person.RelatedFrom = relationships.Where(r => r.RelatedId == personId).ToList();

        _response.Result = _mapper.Map<PersonDto>(person);
        _response.IsSuccess = true;
        _response.StatusCode = System.Net.HttpStatusCode.OK;
        return _response;
    }
}