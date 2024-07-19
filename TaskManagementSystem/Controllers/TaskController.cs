using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Data;
using TaskManagementSystem.Services.Interfaces;

namespace TaskManagementSystem.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        //Imporvements: Instead of outputting Ok() we can output the object that was created with hateoas links all all related APIs

        [Authorize(Roles = "Admin,Employee,Manager")]
        [HttpPost]
        public async Task<IActionResult> CreateTaskAsync([FromBody] TaskEntity task)
        {
            try
            {
                await _taskService.CreateTaskAsync(task);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while creating the task: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin,Employee,Manager")]
        [HttpPut("{taskId}")]
        public async Task<IActionResult> UpdateTaskAsync(int taskId, [FromBody] TaskEntity task)
        {
            try
            {
                task.TaskId = taskId;
                await _taskService.UpdateTaskAsync(task);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the task: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin,Employee,Manager")]
        [HttpDelete("{taskId}")]
        public async Task<IActionResult> DeleteTaskAsync(int taskId)
        {
            try
            {
                await _taskService.DeleteTaskAsync(taskId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting the task: {ex.Message}");
            }
        }

        #region Employee Actions
        [Authorize(Roles = "Admin,Employee,Manager")]
        [HttpPost("attachments")]
        public async Task<IActionResult> AddAttachmentAsync([FromBody] TaskAttachmentEntity attachmentEntity)
        {
            try
            {
                await _taskService.AddAttachmentAsync(attachmentEntity);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while adding the attachment: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin,Employee,Manager")]
        [HttpPost("notes")]
        public async Task<IActionResult> AddNoteAsync([FromBody] TaskNoteEntity noteEntity)
        {
            try
            {
                await _taskService.AddNoteAsync(noteEntity);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while adding the note: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin,Employee,Manager")]
        [HttpPut("complete/{taskId}")]
        public async Task<IActionResult> CompleteTaskAsync(int taskId)
        {
            try
            {
                await _taskService.CompleteTaskAsync(taskId);
                return Ok();
            }
            catch (Exception ex)
            {
            return StatusCode(500, $"An error occurred while completing the task: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin,Employee,Manager")]
        [HttpGet("employee/{employeeId}")]
        public async Task<IActionResult> GetTasksForEmployeeAsync(int employeeId)
        {
            try
            {
                var tasks = await _taskService.GetTasksForEmployeeAsync(employeeId);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving tasks for employee: {ex.Message}");
            }
        }

        #endregion

        #region Manager Access

        [Authorize(Roles = "Manager")]
        [HttpGet("manager/team/{teamId}")]
        public async Task<IActionResult> GetTasksByTeamForManagerAsync(int teamId)
        {
            try
            {
                bool includeChildObject = true;
                var tasks = await _taskService.GetTasksForEachTeamAsync(teamId, includeChildObject);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving tasks for team: {ex.Message}");
            }
        }

        #endregion

        #region Admin Access

        // Fetch tasks which are due in this week (Ex: Sun-Sat)
        [Authorize(Roles = "Admin")]
        [HttpGet("admin/due-in-week")]
        public async Task<IActionResult> GetTasksDueInWeekAsync()
        {
            try
            {
                var tasks = await _taskService.GetTasksDueInWeekAsync();
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving tasks due in a week: {ex.Message}");
            }
        }

        // Fetch tasks which are due in this Month (Ex: July 1 to July 31)
        [Authorize(Roles = "Admin")]
        [HttpGet("admin/due-in-month")]
        public async Task<IActionResult> GetTasksDueInMonthAsync()
        {
            try
            {
                var tasks = await _taskService.GetTasksDueInMonthAsync();
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving tasks due in a month: {ex.Message}");
            }
        }

        [HttpGet("admin/team/{teamId}")]
        [Authorize(Roles = "Admin")]        
        public async Task<IActionResult> GetTasksByTeamForAdminAsync(int teamId)
        {
            try
            {
                bool includeChildObject = false;
                var tasks = await _taskService.GetTasksForEachTeamAsync(teamId, includeChildObject);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving tasks for team: {ex.Message}");
            }
        }

        [HttpGet("admin/all")]
        [Authorize(Roles = "Admin")]        
        public async Task<IActionResult> GetAllTasksAsync()
        {
            try
            {
                var tasks = await _taskService.GetAllTasksAsync();
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving all tasks: {ex.Message}");
            }
        }
        
        #endregion
    }
}
