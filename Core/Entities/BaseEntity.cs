namespace Core.Entities;

public class BaseEntity
{
    public int Id { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; protected set; }

    public void MarkAsUpdated()
    {
        UpdatedAt = DateTime.UtcNow;
    }
    
    protected BaseEntity()
    {
        var now = DateTime.UtcNow;
        CreatedAt = now;
        UpdatedAt = now;
    }
}