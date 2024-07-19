using TaskManagementSystem.Data;

namespace TaskManagementSystem.Repositories.Interfaces
{
    public interface ITaskRepository
    {
        Task CreateTaskAsync(TaskEntity task);
        Task UpdateTaskAsync(TaskEntity task);
        Task DeleteTaskAsync(int taskId);
        Task AddAttachmentAsync(TaskAttachmentEntity attachmentEntity);
        Task AddNoteAsync(TaskNoteEntity noteEntity);
        Task CompleteTaskAsync(int taskId);
        Task<List<TaskEntity>> GetTasksForEmployeeAsync(int employeeId);

         #region Manager & Admin Access
        Task<List<TaskEntity>> GetTasksForEachTeamAsync(int teamId, bool includeChildObject);
        #endregion

        #region Admin Access
        Task<List<(int?, TaskEntity)>> GetAllTasksAsync();
        Task<List<(int?, TaskEntity)>> GetTasksDueInMonthAsync();
        Task<List<(int?, TaskEntity)>> GetTasksDueInWeekAsync();
        #endregion
    }
}
