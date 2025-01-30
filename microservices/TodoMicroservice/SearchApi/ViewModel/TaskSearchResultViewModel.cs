namespace SearchApi.ViewModel;

public sealed class TaskSearchResultViewModel
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public bool Completed { get; set; }
}