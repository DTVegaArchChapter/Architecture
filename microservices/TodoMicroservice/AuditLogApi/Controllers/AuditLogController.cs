namespace AuditLogApi.Controllers
{
    using AuditLogApi.Infrastructure.Data;

    using Microsoft.AspNetCore.Mvc;

    using MongoDB.Bson;

    [ApiController]
    [Route("[controller]")]
    public class AuditLogController : ControllerBase
    {
        private readonly IRepository _repository;

        public AuditLogController(IRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet("GetCollections")]
        public IEnumerable<string> GetCollections()
        {
            return _repository.GetDocumentNames();
        }

        [HttpGet("List/{collectionName}")]
        public IEnumerable<string> List(string collectionName)
        {
            return _repository.List(collectionName);
        }
    }
}