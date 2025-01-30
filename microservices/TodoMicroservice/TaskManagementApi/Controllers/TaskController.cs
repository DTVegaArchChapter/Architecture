namespace TaskManagementApi.Controllers
{
    using MessageQueue;

    using Microsoft.AspNetCore.Mvc;

    using TaskManagementApi.Events;
    using TaskManagementApi.Infrastructure;
    using TaskManagementApi.ViewModel;

    [ApiController]
    [Route("[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly TaskDbContext _taskDbContext;

        private readonly IMessageQueuePublisherService _messageQueuePublisherService;

        public TaskController(TaskDbContext taskDbContext, IMessageQueuePublisherService messageQueuePublisherService)
        {
            _taskDbContext = taskDbContext ?? throw new ArgumentNullException(nameof(taskDbContext));
            _messageQueuePublisherService = messageQueuePublisherService ?? throw new ArgumentNullException(nameof(messageQueuePublisherService));
        }

        [Produces("application/json")]
        [HttpGet("GetTasks", Name = "GetTasks")]
        public IEnumerable<TaskListItemViewModel> Get()
        {
            var result = _taskDbContext.Tasks.Select(x => new TaskListItemViewModel { Id = x.Id, Title = x.Title, Completed = x.Completed }).OrderBy(x => x.Id).ToList();
            return result;
        }

        [HttpPost("AddTask", Name = "AddTask")]
        public async Task Add(TaskAddViewModel addViewModel)
        {
            var taskEntity = new TaskEntity { Title = addViewModel.Title };

            await _taskDbContext.Tasks.AddAsync(taskEntity).ConfigureAwait(false);
            await _taskDbContext.SaveChangesAsync().ConfigureAwait(false);

            await _messageQueuePublisherService.PublishMessage(new TaskAddedEvent(taskEntity.Id, taskEntity.Title)).ConfigureAwait(false);
        }

        [HttpPost("ChangeCompleted/{taskId}", Name = "ChangeCompleted")]
        public async Task<ActionResult> ChangeCompleted(int taskId, bool completed)
        {
            var taskEntity = await _taskDbContext.Tasks.FindAsync(taskId).ConfigureAwait(false);
            if (taskEntity == null)
            {
                return NotFound();
            }

            taskEntity.Completed = completed;
            await _taskDbContext.SaveChangesAsync().ConfigureAwait(false);

            await _messageQueuePublisherService.PublishMessage(new TaskStatusChangedEvent(taskEntity.Id, completed)).ConfigureAwait(false);

            return Ok();
        }

        [HttpPost("UpdateTask", Name = "UpdateTask")]
        public async Task<ActionResult> UpdateTask(TaskUpdateViewModel updateViewModel)
        {
            var taskEntity = await _taskDbContext.Tasks.FindAsync(updateViewModel.TaskId).ConfigureAwait(false);
            if (taskEntity == null)
            {
                return NotFound();
            }

            taskEntity.Title = updateViewModel.Title;
            await _taskDbContext.SaveChangesAsync().ConfigureAwait(false);

            await _messageQueuePublisherService.PublishMessage(new TaskUpdatedEvent(taskEntity.Id, taskEntity.Title)).ConfigureAwait(false);

            return Ok();
        }

        [HttpPost("DeleteTask/{taskId}", Name = "DeleteTask")]
        public async Task<ActionResult> DeleteTask(int taskId)
        {
            var taskEntity = new TaskEntity { Id = taskId };
            _taskDbContext.Tasks.Attach(taskEntity);
            _taskDbContext.Tasks.Remove(taskEntity);
            await _taskDbContext.SaveChangesAsync().ConfigureAwait(false);

            await _messageQueuePublisherService.PublishMessage(new TaskDeletedEvent(taskEntity.Id)).ConfigureAwait(false);

            return Ok();
        }
    }
}