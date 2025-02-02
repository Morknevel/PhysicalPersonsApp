using System.Net;

namespace Dal.Models;

public class ApiResponse
{
    public HttpStatusCode StatusCode { get; set; }
    public bool IsSuccess { get; set; } = true;
    public List<String> ErrorMessages { get; set; }
    public Object  Result { get; set; }
}