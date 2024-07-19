using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementSystem.Data;

namespace TaskManagementSystem.Model.Response.TaskController
{
    public class GetAllTasksModel
    {
        public int? TeamId { get; set; }
        public int TotalTasks { get; set; }
        public int CompletedOnTime { get; set; }
        public int CompletedLate { get; set; }
        public int InProgress { get; set; }
        public int PastDue { get; set; }
        public List<TaskEntity>? Tasks { get; set; }
    }
}