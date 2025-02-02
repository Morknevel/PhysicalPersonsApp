using Dal.Models;
using Dal.Models.DTO.CreateDTO;

namespace Bal.ServiceContracts;

public interface ICityService
{
    Task<ApiResponse> AddCityAsync(CityCreateDto cityCreateDto);
}