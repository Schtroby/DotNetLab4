using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabIV.Models;

namespace LabIV.Models
{
    public class Comment
    {
        
        public int Id { get; set; }
        public string Text { get; set; }
        public bool Important { get; set; }
        public Task Task { get; set; }
        public User Owner { get; set; }
       
    }
}
