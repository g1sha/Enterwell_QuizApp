namespace Core.Entities;

public class BaseEntity
{
    public int Id { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; protected set; }
    public string? CreatedBy { get; private set; }
    public string? UpdatedBy { get; protected set; }

    public void MarkAsUpdated(string updatedByUserId)
    {
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedByUserId;
    }
    
    protected BaseEntity()
    {
        var now = DateTime.UtcNow;
        CreatedAt = now;
        UpdatedAt = now;
    }
    
    public void SetCreatedBy(string userId)
    {
        CreatedBy = userId;
    }
}