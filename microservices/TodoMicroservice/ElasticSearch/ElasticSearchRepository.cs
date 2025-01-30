namespace ElasticSearch;

using Nest;

public interface IElasticSearchRepository
{
    void IndexDocument<TDocument>(TDocument document)
        where TDocument : class;

    TDocument? Get<TDocument>(DocumentPath<TDocument> id)
        where TDocument : class;

    void Update<TDocument>(DocumentPath<TDocument> id, TDocument document)
        where TDocument : class;

    void Delete<TDocument>(DocumentPath<TDocument> id)
        where TDocument : class;
}

public sealed class ElasticSearchRepository : IElasticSearchRepository
{
    private readonly IElasticClient _elasticClient;

    public ElasticSearchRepository(IElasticClient elasticClient)
    {
        _elasticClient = elasticClient ?? throw new ArgumentNullException(nameof(elasticClient));
    }

    public void IndexDocument<TDocument>(TDocument document)
        where TDocument : class
    {
        if (document == null)
        {
            throw new ArgumentNullException(nameof(document));
        }

        var indexResponse = _elasticClient.IndexDocument(document);
        if (!indexResponse.IsValid || indexResponse.Result != Result.Created)
        {
            var errorMessage = $"{document.GetType()} document could not be indexed";
            if (indexResponse.OriginalException != null)
            {
                throw new InvalidOperationException(errorMessage, indexResponse.OriginalException);
            }

            throw new InvalidOperationException(errorMessage);
        }
    }

    public TDocument? Get<TDocument>(DocumentPath<TDocument> id)
        where TDocument : class
    {
        if (id == null)
        {
            throw new ArgumentNullException(nameof(id));
        }

        var getResponse = _elasticClient.Get(id);
        if (!getResponse.IsValid)
        {
            var errorMessage = "Document could not be found";
            if (getResponse.OriginalException != null)
            {
                throw new InvalidOperationException(errorMessage, getResponse.OriginalException);
            }

            throw new InvalidOperationException(errorMessage);
        }

        return !getResponse.Found ? null : getResponse.Source;
    }

    public void Update<TDocument>(DocumentPath<TDocument> id, TDocument document)
        where TDocument : class
    {
        var documentUpdateResponse = _elasticClient.Update(id, x => x.Doc(document));

        if (!documentUpdateResponse.IsValid || (documentUpdateResponse.Result != Result.Updated && documentUpdateResponse.Result != Result.Noop))
        {
            var errorMessage = "Document could not be updated";
            if (documentUpdateResponse.OriginalException != null)
            {
                throw new InvalidOperationException(errorMessage, documentUpdateResponse.OriginalException);
            }

            throw new InvalidOperationException(errorMessage);
        }
    }

    public void Delete<TDocument>(DocumentPath<TDocument> id)
        where TDocument : class
    {
        var documentDeleteResponse = _elasticClient.Delete(id);
        if (documentDeleteResponse.IsValid && documentDeleteResponse.Result != Result.Error)
        {
            return;
        }

        var errorMessage = $"{id} document could not be deleted";
        if (documentDeleteResponse.OriginalException != null)
        {
            throw new InvalidOperationException(errorMessage, documentDeleteResponse.OriginalException);
        }

        throw new InvalidOperationException(errorMessage);
    }
}