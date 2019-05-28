using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabIV.DTO
{
    public class CommentFilterDTO
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool Important { get; set; }
        public int TaskId { get; set; }
    }
}
