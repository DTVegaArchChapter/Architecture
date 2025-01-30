namespace TaskManagementApi.ViewModel;

using System.ComponentModel.DataAnnotations;

public sealed class TaskUpdateViewModel
{
    public int TaskId { get; set; }

    [StringLength(300)]
    [Required]
    public string? Title { get; set; }
}