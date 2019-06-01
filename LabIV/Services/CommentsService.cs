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

        Comment Create(CommentPostDTO task, User addedBy);

        Comment Upsert(int id, Comment comment);

        Comment Delete(int id);

        Comment GetById(int id);
    }

    public class CommentsService : ICommentsService
    {
        private TasksDbContext context;

        public CommentsService(TasksDbContext context)
        {
            this.context = context;
        }

        public Comment Create(CommentPostDTO comment, User addedBy)
        {
            Comment commentAdd = CommentPostDTO.ToComment(comment);
            commentAdd.Owner = addedBy;
            context.Comments.Add(commentAdd);
            context.SaveChanges();
            return commentAdd;
        }

        public Comment Delete(int id)
        {
            var existing = context.Comments.FirstOrDefault(comment => comment.Id == id);
            if (existing == null)
            {
                return null;
            }
            context.Comments.Remove(existing);
            context.SaveChanges();
            return existing;
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

        public Comment GetById(int id)
        {
            return context.Comments.FirstOrDefault(c => c.Id == id);
        }

        public Comment Upsert(int id, Comment comment)
        {
            var existing = context.Comments.AsNoTracking().FirstOrDefault(c => c.Id == id);
            if (existing == null)
            {
                context.Comments.Add(comment);
                context.SaveChanges();
                return comment;

            }

            comment.Id = id;
            context.Comments.Update(comment);
            context.SaveChanges();
            return comment;
        }
    }
}
