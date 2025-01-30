namespace AuditLogApi.Infrastructure.Data;

using System.Collections.Concurrent;

using MongoDB.Bson;
using MongoDB.Driver;

public sealed class MongoDbRepository : IRepository
{
    private readonly MongoDbSettings _mongoDbSettings;

    private readonly IMongoDatabase _database;

    private readonly ConcurrentDictionary<string, IMongoCollection<BsonDocument>> _collections = new();

    public MongoDbRepository(MongoDbSettings mongoDbSettings)
    {
        _mongoDbSettings = mongoDbSettings ?? throw new ArgumentNullException(nameof(mongoDbSettings));

        var mongoClient = new MongoClient(_mongoDbSettings.ConnectionString);
        _database = mongoClient.GetDatabase(_mongoDbSettings.Database);

    }

    public IList<string> GetDocumentNames(CancellationToken cancellationToken = default)
    {
        var result = new List<string>();
        var iterator = _database.ListCollectionNames(cancellationToken: cancellationToken);
        while (iterator.MoveNext())
        {
            result.AddRange(iterator.Current);
        }

        return result;
    }

    public IList<string> List(string documentName)
    {
        if (string.IsNullOrWhiteSpace(documentName))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(documentName));
        }

        var collection = _collections.GetOrAdd(documentName, s => _database.GetCollection<BsonDocument>(s));
        return collection.Find(_ => true)
                  .Sort(new SortDefinitionBuilder<BsonDocument>().Descending("$natural"))
                  .Limit(10)
                  .ToList()
                  .Select(x => x.ToJson())
                  .ToList();
    }
}