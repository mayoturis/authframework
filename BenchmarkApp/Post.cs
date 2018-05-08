using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BenchmarkApp
{
    class Post
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime Published { get; set; }
        public bool Deleted { get; set; }        
        

        public User User { get; set; }        
        public List<Comment> Comments { get; set; }
    }
}