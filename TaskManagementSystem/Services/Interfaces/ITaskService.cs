using TaskManagementSystem.Data;
using TaskManagementSystem.Model.Response;
using TaskManagementSystem.Model.Response.TaskController;

namespace TaskManagementSystem.Services.Interfaces
{
    public interface ITaskService
    {
        Task CreateTaskAsync(TaskEntity task);
        Task UpdateTaskAsync(TaskEntity task);
        Task DeleteTaskAsync(int taskId);
        Task AddAttachmentAsync(TaskAttachmentEntity attachmentEntity);
        Task AddNoteAsync(TaskNoteEntity noteEntity);
        Task CompleteTaskAsync(int taskId);
        Task<List<TaskEntity>> GetTasksForEmployeeAsync(int employeeId);
        Task<List<TaskEntity>> GetTasksForEachTeamAsync(int teamId, bool includeChildObject);
        Task<List<GetAllTasksModel>> GetAllTasksAsync();
        Task<List<GetTasksDueInWeekModel>> GetTasksDueInWeekAsync();
        Task<List<GetTasksDueInMonthModel>> GetTasksDueInMonthAsync();
    }
}
