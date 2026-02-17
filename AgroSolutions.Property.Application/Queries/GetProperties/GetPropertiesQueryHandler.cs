using AgroSolutions.Property.Application.DTOs;
using AgroSolutions.Property.Infrastructure.Persistence;
using MediatR;
using Serilog;

namespace AgroSolutions.Property.Application.Queries.GetProperties;

public class GetPropertiesQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetPropertiesQuery, IEnumerable<GetPropertiesQueryResult>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<IEnumerable<GetPropertiesQueryResult>> Handle(GetPropertiesQuery request, CancellationToken cancellationToken)
    {
        Log.Information("Started the get properties.");

        List<Domain.Entities.Property> properties = await _unitOfWork.Properties.GetAllPropertiesWithFieldsAndCropByUserIdNoTrackingAsync(request.UserId, cancellationToken);
        Log.Information("Properties found in a total of {TotalRecords} records.", properties.Count);

        Log.Information("Finished the get properties.");
        return properties.Select(p => new GetPropertiesQueryResult(
            p.PropertyId,
            p.UserId,
            p.Name,
            p.Description,
            p.Fields.Select(f => new FieldDto(
                f.FieldId,
                f.Name,
                f.Crop.Name,
                f.TotalAreaInHectares
            ))
        ));
    }
}
