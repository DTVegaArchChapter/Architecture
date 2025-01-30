namespace SearchApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Nest;

    using SearchApi.ViewModel;

    [ApiController]
    [Route("[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly IElasticClient _elasticClient;

        public SearchController(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        [Produces("application/json")]
        [HttpPost(Name = "Search")]
        public IEnumerable<TaskSearchResultViewModel> Search([FromBody]TaskSearchViewModel searchViewModel)
        {
            var searchResponse = _elasticClient.Search<Infrastructure.Model.Task>(
                s => s.Query(
                    q => q
                        .Match(m => 
                            m.Field(f => f.Title)
                                .Query(searchViewModel.Text)) &&
                         q.ConstantScore(m =>
                             m.Filter(f => 
                                 f.Term(t => t.Completed, searchViewModel.Completed))))
                    .Sort(d => d.Ascending(f => f.Id)));

            return searchResponse.Documents.Select(x => new TaskSearchResultViewModel { Id = x.Id, Completed = x.Completed, Title = x.Title});
        }
    }
}