using System.ComponentModel.DataAnnotations;

namespace Dal.CustomValidations;

public class MinimumAgeValidationAttribute : ValidationAttribute
{
    private int _minimumAge;

    public MinimumAgeValidationAttribute(int minimumAge)
    {
        _minimumAge = minimumAge;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is DateTime dateOfBirth)
        {
            if (dateOfBirth > DateTime.Now.AddYears(-_minimumAge))
            {
                return new ValidationResult($"Must be at least {_minimumAge} years old.");
            }
        }
        return ValidationResult.Success;
    }
}