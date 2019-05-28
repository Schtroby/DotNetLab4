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
    public interface ICommentsService
    {
        IEnumerable<CommentFilterDTO> GetAll(String keyword);
    }

    public class CommentsService : ICommentsService
    {
        private TasksDbContext context;

        public CommentsService(TasksDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<CommentFilterDTO> GetAll(String keyword)
        {
            IQueryable<Task> result = context.Tasks.Include(c => c.Comments);

            List<CommentFilterDTO> resultFilteredComments = new List<CommentFilterDTO>();
            List<CommentFilterDTO> resultAllComments = new List<CommentFilterDTO>();

            foreach (Task task in result)
            {
                task.Comments.ForEach(c =>
                {
                    if (c.Text == null || keyword == null)
                    {
                        CommentFilterDTO comment = new CommentFilterDTO
                        {
                            Id = c.Id,
                            Important = c.Important,
                            Text = c.Text,
                            TaskId = task.Id

                        };
                        resultAllComments.Add(comment);
                    }
                    else if (c.Text.Contains(keyword))
                    {
                        CommentFilterDTO comment = new CommentFilterDTO
                        {
                            Id = c.Id,
                            Important = c.Important,
                            Text = c.Text,
                            TaskId = task.Id

                        };
                        resultFilteredComments.Add(comment);

                    }
                });
            }
            if (keyword == null)
            {
                return resultAllComments;
            }
            return resultFilteredComments;
        }
    }
}
