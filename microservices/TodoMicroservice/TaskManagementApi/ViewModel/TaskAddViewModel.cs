namespace TaskManagementApi.ViewModel;

using System.ComponentModel.DataAnnotations;

public sealed class TaskAddViewModel
{
    [StringLength(300)]
    [Required]
    public string? Title { get; set; }
}