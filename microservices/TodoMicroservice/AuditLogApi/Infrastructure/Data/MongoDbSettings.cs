namespace AuditLogApi.Infrastructure.Data
{
    public sealed class MongoDbSettings
    {
        public string ConnectionString { get; set; }

        public string Database { get; set; }
    }
}
