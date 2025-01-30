namespace AuditLogApi.Infrastructure.Data
{
    using MongoDB.Bson;

    public interface IRepository
    {
        IList<string> GetDocumentNames(CancellationToken cancellationToken = default);

        IList<string> List(string documentName);
    }
}
