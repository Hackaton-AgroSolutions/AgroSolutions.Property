using MediatR;

namespace AgroSolutions.Property.Application.Queries.GetProperties;

public record GetPropertiesQuery(int UserId) : IRequest<IEnumerable<GetPropertiesQueryResult>>;
