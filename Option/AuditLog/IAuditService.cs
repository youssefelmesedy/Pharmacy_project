namespace Option.AuditLog
{
    public interface IAuditService
    {
        Task LogAsync(AuditLogEntry entry);
    }

}
