namespace AuditLogWorkerService.Infrastructure.Data
{
    using MongoDB.Bson;

    public interface IRepository
    {
        void Insert(string documentName, BsonDocument document, CancellationToken cancellationToken = default);
    }
}
