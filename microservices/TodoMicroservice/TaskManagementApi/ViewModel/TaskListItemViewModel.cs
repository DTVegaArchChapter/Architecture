namespace TaskManagementApi.ViewModel
{
    public sealed class TaskListItemViewModel
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public bool Completed { get; set; }
    }
}
