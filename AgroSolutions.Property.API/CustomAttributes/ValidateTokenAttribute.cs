using AgroSolutions.Property.API.Filters;
using Microsoft.AspNetCore.Mvc;

namespace AgroSolutions.Property.API.CustomAttributes;

public class ValidateTokenAttribute : TypeFilterAttribute
{
    public ValidateTokenAttribute() : base(typeof(ValidateTokenFilter))
    {
    }
}
