namespace AgroSolutions.Property.Domain.Entities;

public class Property(int userId, string name, string? description)
{
    public int PropertyId { get; private set; }
    public int UserId { get; private set; } = userId;
    public string Name { get; private set; } = name;
    public string? Description { get; private set; } = description;
    public virtual ICollection<Field> Fields { get; private set; } = [];

    public void Update(string name, string? description)
    {
        Name = name;
        Description = description;
    }
}
