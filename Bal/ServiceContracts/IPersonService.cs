using Dal.Models;
using Dal.Models.DTO.CreateDTO;
using Dal.Models.DTO.UpdateDTO;
using Dal.Paging;
using Microsoft.AspNetCore.Http;

namespace Bal.ServiceContracts;

public interface IPersonService
{
    Task<ApiResponse> AddPersonAsync(PersonCreateDto person);
    Task<ApiResponse> UpdatePersonAsync(int id,PersonUpdateDto person);
    Task<ApiResponse> GetPersonDetailsAsync(int personId);
    Task<ApiResponse> DeletePersonAsync(int personId);
    Task<ApiResponse> UpdatePersonPhotoAsync(int personId, IFormFile file);
    Task<ApiResponse> DeletePersonPhotoAsync(int personId);
    Task<ApiResponse> QuickSearchAsync(string searchTerm);
    Task<ApiResponse> DetailedSearchAsync(PersonSearchParameters parameters, int pageNumber = 1, int pageSize = 10);
 
}