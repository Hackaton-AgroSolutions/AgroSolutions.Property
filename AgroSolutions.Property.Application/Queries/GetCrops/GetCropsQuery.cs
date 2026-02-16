using MediatR;

namespace AgroSolutions.Property.Application.Queries.GetCrops;

public record GetCropsQuery : IRequest<IEnumerable<GetCropQueryResult>>;
