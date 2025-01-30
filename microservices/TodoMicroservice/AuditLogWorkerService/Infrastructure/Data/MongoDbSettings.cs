namespace AuditLogWorkerService.Infrastructure.Data
{
    public sealed class MongoDbSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string Database { get; set; } = null!;
    }
}
