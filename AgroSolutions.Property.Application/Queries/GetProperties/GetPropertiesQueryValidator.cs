using AgroSolutions.Property.Application.Validators;
using FluentValidation;
using Serilog;

namespace AgroSolutions.Property.Application.Queries.GetProperties;

public class GetPropertiesQueryValidator : AbstractValidator<GetPropertiesQuery>
{
    public GetPropertiesQueryValidator()
    {
        Log.Information("Starting the validator {ValidatorName}.", nameof(GetPropertiesQueryValidator));

        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(q => q.UserId).ValidUserId();
    }
}
