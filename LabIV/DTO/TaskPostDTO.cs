using LabIV.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Task = LabIV.Models.Task;


namespace LabIV.DTO
{
    public class TaskPostDTO
    {

        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime Deadline { get; set; }

        [EnumDataType(typeof(TaskImportance))]
        public string TaskImportance { get; set; }

        [EnumDataType(typeof(TaskState))]
        public string TaskState { get; set; }

        public DateTime? DateClosed { get; set; }

        public List<Comment> Comments { get; set; }

        public static Task ToTask(TaskPostDTO task)
        {
            TaskImportance TaskImportance = Models.TaskImportance.Low;
            if (task.TaskImportance == "Medium")
            {
                TaskImportance = Models.TaskImportance.Medium;
            }
            else if (task.TaskImportance == "Hight")
            {
                TaskImportance = Models.TaskImportance.Hight;
            }

            TaskState TaskState = Models.TaskState.Open;
            if (task.TaskState == "InProgress")
            {
                TaskState = Models.TaskState.InProgress;
            }
            else if (task.TaskState == "Closed")
            {
                TaskState = Models.TaskState.Closed;
            }

            return new Task
            {
                Title = task.Title,
                Description = task.Description,
                DateAdded = task.DateAdded,
                Deadline = task.Deadline,
                TaskImportance = TaskImportance,
                TaskState = TaskState,
                DateClosed = task.DateClosed,


            };

        }
    }
}
