using System.Net;
using AutoMapper;
using Bal.ServiceContracts;
using Dal.Contracts;
using Dal.Models;
using Dal.Models.DTO;
using Dal.Models.DTO.CreateDTO;

namespace Bal.Services;

public class CityService : ICityService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ApiResponse _response;

    public CityService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _response = new();
    }
    
    public async Task<ApiResponse> AddCityAsync(CityCreateDto cityCreateDto)
    { 
        if (await _unitOfWork.Cities.AnyAsync(c => c.Name== cityCreateDto.Name))
        {
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.ErrorMessages = ["A city with the same name already exists"];
            return _response;
        }
        
        try
        {
            var city = _mapper.Map<City>(cityCreateDto);
            await _unitOfWork.Cities.CreateCityAsync(city);
            _response.Result = _mapper.Map<CityDto>(city);
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
}