
namespace BulkUpdatesDeletes.Model;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsInactive { get; set; }
    public DateTime LastLogin { get; set; }
}
