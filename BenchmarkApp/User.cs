using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BenchmarkApp
{
    class User
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }        
        public string Email { get; set; }
        public DateTime Created { get; set; }
        public bool Active { get; set; }

        public List<Post> Posts { get; set; }
        public List<Comment> Comments { get; set; }

        [NotMapped] //  not stored in DB
        public string Password { get; set; }
    }
}