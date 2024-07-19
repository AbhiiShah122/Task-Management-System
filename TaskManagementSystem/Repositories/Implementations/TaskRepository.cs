using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Data;
using TaskManagementSystem.Data.Context;
using TaskManagementSystem.Repositories.Interfaces;

namespace TaskManagementSystem.Repositories.Implementations
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskManagementContext _context;

        public TaskRepository(TaskManagementContext context)
        {
            _context = context;
        }

        async Task ITaskRepository.CreateTaskAsync(TaskEntity task)
        {
            try
            {
                _context.Tasks.Add(task);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while creating the task: {ex.Message}");
                throw;
            }
        }

        async Task ITaskRepository.UpdateTaskAsync(TaskEntity task)
        {
            try
            {
                _context.Tasks.Update(task);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating the task: {ex.Message}");
                throw;
            }
        }

        async Task ITaskRepository.DeleteTaskAsync(int taskId)
        {
            try
            {
                var taskEntity = await _context.Tasks.FindAsync(taskId);
                if (taskEntity != null)
                {
                    _context.Tasks.Remove(taskEntity);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception($"Task with ID {taskId} not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deleting the task: {ex.Message}");
                throw;
            }
        }
        
        #region Employee Actions

        public async Task AddAttachmentAsync(TaskAttachmentEntity attachmentEntity)
        {
            try
            {
                _context.TaskAttachments.Add(attachmentEntity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while adding the attachment: {ex.Message}");
                throw;
            }
        }

        public async Task AddNoteAsync(TaskNoteEntity noteEntity)
        {
            try
            {
                _context.TaskNotes.Add(noteEntity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while adding the note: {ex.Message}");
                throw;
            }
        }
        
        public async Task CompleteTaskAsync(int taskId)
        {
            try
            {
                var task = await _context.Tasks.FindAsync(taskId);
                if (task != null)
                {
                    task.IsCompleted = true;
                    task.CompletedOn = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception($"Task with ID {taskId} not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while completing the task: {ex.Message}");
                throw;
            }
        }

        async Task<List<TaskEntity>> ITaskRepository.GetTasksForEmployeeAsync(int employeeId)
        {
            try
            {
                var tasks = await _context.Tasks
                    .Include(task => task.Attachments)
                    .Include(task => task.Notes)
                    .Where(task => task.EmployeeId == employeeId)
                    .ToListAsync();

                return tasks;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving tasks for employee {employeeId}: {ex.Message}");
                throw;
            }
        }

        #endregion
        #region Manager & Admin Access
        async Task<List<TaskEntity>> ITaskRepository.GetTasksForEachTeamAsync(int teamId, bool includeChildObject)
        {
            try
            {
                var teammembers = await _context.Users
                    .Where(user => user.TeamId == teamId)
                    .Select(user => user.UserId)
                    .ToListAsync();

                if(includeChildObject)
                {
                    var tasks = await _context.Tasks
                        .Where(task => teammembers.Contains(task.EmployeeId))
                        .Include(task => task.Attachments)
                        .Include(task => task.Notes)
                        .ToListAsync();
                    return tasks;
                }
                //Not Including Attachments and Notes as they are not required for this operation.
                else
                {
                    var tasks = await _context.Tasks
                        .Where(task => teammembers.Contains(task.EmployeeId))
                        .ToListAsync();

                    return tasks;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving tasks for team of manager {teamId}: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region Admin Access Only

        async Task<List<(int?, TaskEntity)>> ITaskRepository.GetTasksDueInMonthAsync()
        {
            try
            {
                var response = new List<(int?, TaskEntity)>();
                //Data in the database is stored in UTC, hence providing current UTC time. 
                DateTime currentDate = DateTime.UtcNow;
                DateTime startOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
                DateTime endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

                //Not Including Attachments and Notes as they are not required for this operation.
                var tasks = await _context.Tasks
                    .Where(task => task.DueDate >= startOfMonth && task.DueDate <= endOfMonth)
                    .ToListAsync();

                foreach (var task in tasks)
                {
                    var teamId = await _context.Users
                        .Where(user => user.UserId == task.EmployeeId)
                        .Select(user => user.TeamId)
                        .FirstOrDefaultAsync();
                    response.Add((teamId, task));
                }

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving tasks due in the month: {ex.Message}");
                throw;
            }
        }

        async Task<List<(int?, TaskEntity)>> ITaskRepository.GetTasksDueInWeekAsync()
        {
            try
            {
                var response = new List<(int?, TaskEntity)>();
                //Data in the database is stored in UTC, hence providing current UTC time.
                DateTime currentDate = DateTime.UtcNow;
                DateTime startOfWeek = currentDate.Date.AddDays(-(int)currentDate.DayOfWeek);
                DateTime endOfWeek = startOfWeek.AddDays(7);

                //Not Including Attachments and Notes as they are not required for this operation.
                var tasks = await _context.Tasks
                    .Where(task => task.DueDate >= startOfWeek && task.DueDate < endOfWeek)
                    .ToListAsync();

                foreach (var task in tasks)
                {
                    var teamId = await _context.Users
                        .Where(user => user.UserId == task.EmployeeId)
                        .Select(user => user.TeamId)
                        .FirstOrDefaultAsync();
                    response.Add((teamId, task));
                }

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving tasks due in the week: {ex.Message}");
                throw;
            }
        }

        public async Task<List<(int?, TaskEntity)>> GetAllTasksAsync()
        {
            try
            {
                var response = new List<(int?, TaskEntity)>();

                //Not Including Attachments and Notes as they are not required for this operation.
                var tasks = await _context.Tasks.ToListAsync();

                foreach (var task in tasks)
                {
                    var teamId = await _context.Users
                        .Where(user => user.UserId == task.EmployeeId)
                        .Select(user => user.TeamId)
                        .FirstOrDefaultAsync();
                    response.Add((teamId, task));
                }

                return response;
            }
            catch (Exception ex)
            {
            Console.WriteLine($"An error occurred while retrieving all tasks: {ex.Message}");
            throw;
            }
        }

        #endregion
    }
}
