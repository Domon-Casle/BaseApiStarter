namespace CoreDomain.Audit
{
    public enum AuditType
    {
        Read = 0,
        Create = 1,
        Update = 2,
        Delete = 3,
    }

    public class Audit
    {
        public Guid EntityId { get; set; }
        public string EntityType { get; set; }
        public Guid UserId { get; set; }
        public DateTime AuditDate { get; set; }
        public AuditType AuditType { get; set; }
        public string Log { get; set; }

        public Audit(Guid entityId, string entityType, Guid userId, AuditType auditType, string log = "")
        {
            EntityId = entityId;
            EntityType = entityType;
            UserId = userId;
            AuditDate = DateTime.UtcNow;
            AuditType = auditType;
            Log = log;
        }
    }
}
