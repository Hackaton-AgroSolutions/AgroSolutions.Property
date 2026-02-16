namespace AgroSolutions.Property.Domain.Entities;

public class Field
{
    public int FieldId { get; private set; }
    public string Name { get; private set; }
    public int PropertyId { get; private set; }
    public Property Property { get; private set; } = null!;
    public int CropId { get; private set; }
    public Crop Crop { get; private set; } = null!;
    public decimal TotalAreaInHectares { get; private set; }

    public Field()
    {
        Name = string.Empty;
    }

    public Field(int fieldId, string name, Crop crop, decimal totalAreaInHectares)
    {
        FieldId = fieldId;
        Name = name;
        CropId = crop.CropId;
        Crop = crop;
        TotalAreaInHectares = totalAreaInHectares;
    }

    public Field(string name, Crop crop, decimal totalAreaInHectares)
    {
        Name = name;
        CropId = crop.CropId;
        Crop = crop;
        TotalAreaInHectares = totalAreaInHectares;
    }

    public void Update(string name, decimal totalAreaInHectares, Crop crop)
    {
        Name = name;
        TotalAreaInHectares = totalAreaInHectares;
        Crop = crop;
        CropId = crop.CropId;
    }
}
