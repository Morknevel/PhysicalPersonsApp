using System.ComponentModel.DataAnnotations;

namespace Dal.Models.DTO.CreateDTO;

public record CityCreateDto(
    [MaxLength(60, ErrorMessage = "Maximum length for the city name is 60 characters.")]
     string Name 
);