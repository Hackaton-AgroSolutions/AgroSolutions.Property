using AgroSolutions.Property.Application.Validators;
using FluentValidation;
using Serilog;

namespace AgroSolutions.Property.Application.Queries.GetProperty;

public class GetPropertyQueryValidator : AbstractValidator<GetPropertyQuery>
{
    public GetPropertyQueryValidator()
    {
        Log.Information("Starting the validator {ValidatorName}.", nameof(GetPropertyQueryValidator));

        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(q => q.UserId).ValidUserId();
        RuleFor(q => q.PropertyId).ValidPropertyId();
    }
}
