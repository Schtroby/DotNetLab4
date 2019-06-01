using LabIV.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Task = LabIV.Models.Task;


namespace LabIV.DTO
{
    public class TaskGetDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime Deadline { get; set; }

        [EnumDataType(typeof(TaskImportance))]
        public TaskImportance TaskImportance { get; set; }

        [EnumDataType(typeof(TaskState))]
        public TaskState TaskState { get; set; }

        public DateTime? DateClosed { get; set; }

        public int NumberOfComments { get; set; }
        
        public List<Comment> Comments { get; set; }


        public static TaskGetDTO FromTask(Task task)
        {

            return new TaskGetDTO
            {
                Title = task.Title,
                Description = task.Description,
                DateAdded = task.DateAdded,
                Deadline = task.Deadline,
                TaskImportance = task.TaskImportance,
                TaskState = task.TaskState,
                DateClosed = task.DateClosed,
                //Comments = task.Comments,
                NumberOfComments = task.Comments.Count,
                
            };
        }
    }
}
