using System.ComponentModel.DataAnnotations;

namespace Dal.Models.DTO.UpdateDTO;

public class CityUpdateDto
{
    [MaxLength(60, ErrorMessage = "Maximum length for the city name is 60 characters.")]
    public string Name { get; set; }
};