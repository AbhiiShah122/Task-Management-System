using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementSystem.Data;
using TaskManagementSystem.Model.Response;
using TaskManagementSystem.Model.Response.TaskController;
using TaskManagementSystem.Repositories.Interfaces;
using TaskManagementSystem.Services.Interfaces;

namespace TaskManagementSystem.Services.Implementations
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }
        public async Task CreateTaskAsync(TaskEntity task)
        {
            await _taskRepository.CreateTaskAsync(task);
        }

        public async Task UpdateTaskAsync(TaskEntity task)
        {
            await _taskRepository.UpdateTaskAsync(task);
        }

        public async Task DeleteTaskAsync(int taskId)
        {
            await _taskRepository.DeleteTaskAsync(taskId);
        }

        public async Task AddAttachmentAsync(TaskAttachmentEntity attachmentEntity)
        {
            await _taskRepository.AddAttachmentAsync(attachmentEntity);
        }

        public async Task AddNoteAsync(TaskNoteEntity noteEntity)
        {
            await _taskRepository.AddNoteAsync(noteEntity);
        }

        public async Task CompleteTaskAsync(int taskId)
        {
            await _taskRepository.CompleteTaskAsync(taskId);
        }

        public async Task<List<TaskEntity>> GetTasksForEmployeeAsync(int employeeId)
        {
            return await _taskRepository.GetTasksForEmployeeAsync(employeeId);
        }

        public async Task<List<TaskEntity>> GetTasksForEachTeamAsync(int teamId, bool includeChildObject)
        {
            return await _taskRepository.GetTasksForEachTeamAsync(teamId, includeChildObject);
        }

        #region Admin Access
        public async Task<List<GetAllTasksModel>> GetAllTasksAsync()
        {
            var response = await _taskRepository.GetAllTasksAsync();
            var groupedTasks = response.GroupBy(t => t.Item1).ToList();
            var tasksByTeam = new List<GetAllTasksModel>();

            foreach (var group in groupedTasks)
            {
                var teamId = group.Key;
                var tasks = group.ToList();

                var tasksModel = new GetAllTasksModel
                {
                    TeamId = teamId,
                    Tasks = tasks.Select(t => t.Item2).ToList()
                };

                foreach (var task in tasksModel.Tasks)
                {
                    tasksModel.TotalTasks++;

                    if (task.CompletedOn.HasValue)
                    {
                        if (task.CompletedOn <= task.DueDate)
                        {
                            tasksModel.CompletedOnTime++;
                        }
                        else
                        {
                            tasksModel.CompletedLate++;
                        }
                    }
                    else if (task.DueDate < DateTime.Now)
                    {
                        tasksModel.PastDue++;
                    }
                    else
                    {
                        tasksModel.InProgress++;
                    }
                }
                tasksByTeam.Add(tasksModel);
            }

            return tasksByTeam;
        }

        public async Task<List<GetTasksDueInWeekModel>> GetTasksDueInWeekAsync()
        {
            var response = await _taskRepository.GetTasksDueInWeekAsync();
            var groupedTasks = response.GroupBy(t => t.Item1).ToList();
            var tasksByTeam = new List<GetTasksDueInWeekModel>();
            foreach (var group in groupedTasks)
            {
                var teamId = group.Key;
                var tasks = group.ToList();

                var tasksModel = new GetTasksDueInWeekModel
                {
                    TeamId = teamId,
                    Tasks = tasks.Select(t => t.Item2).ToList(),
                    TotalTasks = tasks.Count
                };
                tasksByTeam.Add(tasksModel);
            }
            return tasksByTeam;
        }

        public async Task<List<GetTasksDueInMonthModel>> GetTasksDueInMonthAsync()
        {
            var response = await _taskRepository.GetTasksDueInMonthAsync();
            var groupedTasks = response.GroupBy(t => t.Item1).ToList();
            var tasksByTeam = new List<GetTasksDueInMonthModel>();
            foreach (var group in groupedTasks)
            {
                var teamId = group.Key;
                var tasks = group.ToList();

                var tasksModel = new GetTasksDueInMonthModel
                {
                    TeamId = teamId,
                    Tasks = tasks.Select(t => t.Item2).ToList(),
                    TotalTasks = tasks.Count
                };
                tasksByTeam.Add(tasksModel);
            }
            return tasksByTeam;
        }

        #endregion
    }
}