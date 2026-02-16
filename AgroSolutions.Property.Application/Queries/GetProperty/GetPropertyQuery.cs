using MediatR;

namespace AgroSolutions.Property.Application.Queries.GetProperty;

public record GetPropertyQuery(int UserId, int PropertyId) : IRequest<GetPropertyQueryResult?>;
