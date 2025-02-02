using Bal.ServiceContracts;
using Dal.Models;
using Dal.Models.DTO;
using Dal.Models.DTO.CreateDTO;
using Dal.Models.DTO.UpdateDTO;
using Dal.Paging;
using Microsoft.AspNetCore.Mvc;
using PhysicalPersons.Filters.ActionFilters;

namespace PhysicalPersons.Controllers;

[Route("/api/[controller]")]
[ApiController]
public class PersonApiController : ControllerBase
{
    private readonly IPersonService _personService;
    public PersonApiController(IPersonService personService)
    {
        _personService = personService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ModelValidate]
    public async Task<ActionResult<ApiResponse>> CreatePerson(PersonCreateDto personCreateDto)
    {
        var response = await _personService.AddPersonAsync(personCreateDto);
        if (response.IsSuccess)
        {
            return CreatedAtRoute(nameof(GetPerson), new { id = ((PersonDto)response.Result).PersonId }, response);
        }

        return BadRequest(response.ErrorMessages);
    }

    [HttpGet("{id}", Name = "GetPerson")]
    public async Task<ActionResult<ApiResponse>> GetPerson(int id)
    {
        var response = await _personService.GetPersonDetailsAsync(id);
        if (!response.IsSuccess)
        {
            return NotFound(response.ErrorMessages);
        }
        return Ok(response);
    }

    [HttpPut("{personId:int}", Name = "UpdatePerson")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ModelValidate]
    public async Task<ActionResult<ApiResponse>> UpdatePerson(int personId, [FromBody] PersonUpdateDto personUpdateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var response = await _personService.UpdatePersonAsync(personId, personUpdateDto);
        if (response.IsSuccess)
        {
            return Ok(response);
        }

        return BadRequest(response.ErrorMessages);
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpDelete("/{personId:int}")]
    public async Task<IActionResult> DeletePerson(int personId)
    {
        var response = await _personService.DeletePersonAsync(personId);
        if (response.IsSuccess)
        {
            return Ok(response);
        }

        if (response.ErrorMessages.Contains("Person not found."))
        {
            return NotFound(response);
        }

        return BadRequest(response.ErrorMessages);
    }

    [HttpPost("{personId}/image")]
    public async Task<ActionResult<ApiResponse>> UploadPersonImage(int personId, IFormFile? image)
    {
        if (image == null || image.Length == 0)
        {
            return BadRequest("No file found");
        }

        var response  = await _personService.UpdatePersonPhotoAsync(personId, image);

        if (!response.IsSuccess)
        {
            return BadRequest(response.ErrorMessages);
        }
        
        return Ok(new { Message = "Image uploaded successfully"});
    }

    [HttpDelete("{personId}/image")]
    public async Task<ActionResult<ApiResponse>> DeletePersonImage(int personId)
    {
        var response = await _personService.DeletePersonPhotoAsync(personId);
        if (!response.IsSuccess)
        {
            return BadRequest(response.ErrorMessages);
        }
        return Ok(new { Message = "Image deleted successfully" });
    }

    [HttpGet("search")]
    public async Task<ActionResult<ApiResponse>> QuickSearch(string searchTerm)
    {
        var response = await _personService.QuickSearchAsync(searchTerm);
        if (!response.IsSuccess)
        {
            return BadRequest(response.ErrorMessages);
        }
        return Ok(response);
    }
    [HttpGet("search/detailed")]
    public async Task<ActionResult<ApiResponse>> DetailedSearch([FromQuery] PersonSearchParameters parameters, int pageNumber = 1,
        int pageSize = 10)
    {
        var response = await _personService.DetailedSearchAsync(parameters, pageNumber, pageSize);
        
        if (!response.IsSuccess)
        {
            return BadRequest(response.ErrorMessages);
        }
        return Ok(response);
    }
}