namespace AgroSolutions.Property.Domain.Entities;

public class Crop(int cropId, string name)
{
    public int CropId { get; private set; } = cropId;
    public string Name { get; private set; } = name;
}
