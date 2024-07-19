using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementSystem.Data;

namespace TaskManagementSystem.Model.Response.TaskController
{
    public class GetTasksDueInWeekModel
    {
        public int? TeamId { get; set; }
        public int TotalTasks { get; set; }
        public List<TaskEntity>? Tasks { get; set; }
    }
}