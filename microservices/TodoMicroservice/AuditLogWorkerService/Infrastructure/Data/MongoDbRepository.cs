namespace AuditLogWorkerService.Infrastructure.Data;

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

    public void Insert(string documentName, BsonDocument document, CancellationToken cancellationToken = default)
    {
        if (document == null)
        {
            throw new ArgumentNullException(nameof(document));
        }

        if (string.IsNullOrWhiteSpace(documentName))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(documentName));
        }

        var collection = _collections.GetOrAdd(documentName, s => _database.GetCollection<BsonDocument>(s));
        collection.InsertOne(document, cancellationToken: cancellationToken);
    }
}