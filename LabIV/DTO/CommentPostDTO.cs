using LabIV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Task = LabIV.Models.Task;

namespace LabIV.DTO
{
    public class CommentPostDTO
    {
        public string Text { get; set; }
        public bool Important { get; set; }
        


        public static Comment ToComment(CommentPostDTO comment)
        {
            return new Comment
            {
                Text = comment.Text,
                Important = comment.Important,

          
            };
        }
    }
}
