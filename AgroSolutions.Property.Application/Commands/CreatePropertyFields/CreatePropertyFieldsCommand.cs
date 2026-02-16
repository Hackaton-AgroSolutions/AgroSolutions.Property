using MediatR;

namespace AgroSolutions.Property.Application.Commands.CreatePropertyFields;

public record CreatePropertyFieldData(string Name, int CropId, decimal TotalAreaInHectares);

public record CreatePropertyFieldsCommand(int UserId, int PropertyId, IEnumerable<CreatePropertyFieldData> Fields) : IRequest<IEnumerable<CreatePropertyFieldsCommandResult>?>;
