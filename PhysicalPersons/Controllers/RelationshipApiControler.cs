using Bal.ServiceContracts;
using Dal.Models;
using Dal.Models.DTO.CreateDTO;
using Dal.Models.DTO.DeleteDTO;
using Microsoft.AspNetCore.Mvc;

namespace PhysicalPersons.Controllers;
[ApiController]
[Route("api/[controller]")]

public class RelationshipApiController : ControllerBase
{
    private readonly IRelationshipService _relationshipService;

    public RelationshipApiController(IRelationshipService relationshipService)
    {
        _relationshipService = relationshipService;
    }
    [HttpPost]
    public async Task<ActionResult<ApiResponse>> AddRelationship(RelationshipCreateDto relationshipCreateDto)
    {
        var response = await _relationshipService.AddRelationshipAsync(relationshipCreateDto);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [HttpDelete]
    public async Task<ActionResult<ApiResponse>> DeleteRelationship([FromBody]RelationshipDeleteDto? relationshipDeleteDto)
    {
        if (relationshipDeleteDto == null)
        {
            return BadRequest("Invalid relationship data.");
        }

        try
        {
            var result = await _relationshipService.DeleteRelationshipAsync(relationshipDeleteDto);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        catch (Exception ex)
        { 
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the relationship.");
        }
    }
}