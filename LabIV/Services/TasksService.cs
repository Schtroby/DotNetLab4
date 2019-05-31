using LabIV.DTO;
using LabIV.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task = LabIV.Models.Task;

namespace LabIV.Services
{
    public interface ITasksService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        PaginatedList<TaskGetDTO> GetAll(int page, DateTime? from, DateTime? to);

        Task Create(TaskPostDTO task, User addedBy);

        Task Upsert(int id, Task task);

        Task Delete(int id);

        Task GetById(int id);
    }

    public class TasksService : ITasksService
    {
        private TasksDbContext context;

        public TasksService(TasksDbContext context)
        {
            this.context = context;
        }

        public Task Create(TaskPostDTO task, User addedBy)
        {
            Task taskAdd = TaskPostDTO.ToTask(task);
            task.DateClosed = null;
            task.DateAdded = DateTime.Now;
            taskAdd.Owner = addedBy;
            context.Tasks.Add(taskAdd);
            context.SaveChanges();
            return taskAdd;
        }


        public Task Delete(int id)
        {
            var existing = context.Tasks.Include(t => t.Comments).FirstOrDefault(t => t.Id == id);
            if (existing == null)
            {
                return null;
            }
            context.Tasks.Remove(existing);
            context.SaveChanges();

            return existing;
        }

        public PaginatedList<TaskGetDTO> GetAll(int page, DateTime? from = null, DateTime? to = null)
        {
            IQueryable<Task> result = context
                .Tasks
                .OrderBy(e => e.Id)
                .Include(x => x.Comments);

            PaginatedList<TaskGetDTO> paginatedResult = new PaginatedList<TaskGetDTO>();
            paginatedResult.CurrentPage = page;

            //if (from == null && to == null)
            //    return result;

            if (from != null)
                result = result.Where(t => t.Deadline >= from);

            if (to != null)
                result = result.Where(t => t.Deadline <= to);

            paginatedResult.NumberOfPages = (result.Count() - 1) / PaginatedList<TaskGetDTO>.EntriesPerPage + 1;
            result = result
                .Skip((page - 1) * PaginatedList<TaskGetDTO>.EntriesPerPage)
                .Take(PaginatedList<TaskGetDTO>.EntriesPerPage);
            paginatedResult.Entries = result.Select(e => TaskGetDTO.FromTask(e)).ToList();

            return paginatedResult;
        }

        public Task GetById(int id)
        {
            // sau context.Tasks.Find()
            return context.Tasks
                .Include(c => c.Comments)
                .FirstOrDefault(t => t.Id == id);
        }

        public Task Upsert(int id, Task task)
        {
            var existing = context.Tasks.AsNoTracking().FirstOrDefault(t => t.Id == id);
            if (existing == null)
            {
                task.DateClosed = null;
                task.DateAdded = DateTime.Now;
                context.Tasks.Add(task);
                context.SaveChanges();
                return task;
            }
            task.Id = id;
            if (task.TaskState == TaskState.Closed && existing.TaskState != TaskState.Closed)
                task.DateClosed = DateTime.Now;
            else if (existing.TaskState == TaskState.Closed && task.TaskState != TaskState.Closed)
                task.DateClosed = null;

            context.Tasks.Update(task);
            context.SaveChanges();
            return task;
        }
    }
}
