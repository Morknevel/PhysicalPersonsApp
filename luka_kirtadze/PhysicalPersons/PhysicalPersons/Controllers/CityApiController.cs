using Bal.ServiceContracts;
using Dal.Models;
using Dal.Models.DTO.CreateDTO;
using Microsoft.AspNetCore.Mvc;
using PhysicalPersons.Filters.ActionFilters;

namespace PhysicalPersons.Controllers;
[ApiController]
[Route("api/[controller]")]
public class CityApiController : Controller
{
    private readonly ICityService cityService;

    public CityApiController(ICityService cityService)
    {
        this.cityService = cityService;
    }
    
    [HttpPost]
    [ModelValidate]
    public async Task<ActionResult<ApiResponse>> AddCity(CityCreateDto cityCreateDto)
    {
        var response = await cityService.AddCityAsync(cityCreateDto);
        if (!response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
}