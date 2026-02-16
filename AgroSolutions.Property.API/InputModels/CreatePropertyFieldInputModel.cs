namespace AgroSolutions.Property.API.InputModels;

public record CreatePropertyFieldInputModel(string Name, int CropId, decimal TotalAreaInHectares);
