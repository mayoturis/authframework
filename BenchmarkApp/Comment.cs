using System;
using System.ComponentModel.DataAnnotations;

namespace BenchmarkApp
{
    class Comment
    {
        [Key]
        public int Id { get; set; }
        public string CommentText { get; set; }
        public DateTime CommentDate { get; set; }
        public bool Deleted { get; set; }        

        public Post Post { get; set; }
        public User User { get; set; }
    }
}