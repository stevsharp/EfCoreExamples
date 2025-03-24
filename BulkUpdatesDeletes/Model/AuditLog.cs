
namespace BulkUpdatesDeletes.Model
{
    public class AuditLog
    {
        public int Id { get; set; }
        public string EntityName { get; set; } = string.Empty;
        public int EntityId { get; set; }
        public string ChangeType { get; set; } = string.Empty;
        public DateTime ChangedAt { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
