using AgroSolutions.Property.Domain.Entities;
using AgroSolutions.Property.Infrastructure.Persistence;
using MediatR;
using Serilog;

namespace AgroSolutions.Property.Application.Queries.GetCrops;

public class GetCropsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetCropsQuery, IEnumerable<GetCropQueryResult>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<IEnumerable<GetCropQueryResult>> Handle(GetCropsQuery request, CancellationToken cancellationToken)
    {
        Log.Information("Started the get by all crops.");

        List<Crop> crops = await _unitOfWork.Crops.GetAllAsNoTrackingAsync(cancellationToken);
        Log.Information("Crops found in a total of {TotalRecords} records.", crops.Count);

        Log.Information("Finished the get by all crops.");
        return crops.Select(c => new GetCropQueryResult(c.CropId, c.Name));
    }
}
