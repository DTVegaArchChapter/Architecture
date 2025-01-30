namespace SearchWorkerService.Infrastructure.Model;

public class Task
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public bool Completed { get; set; }
}