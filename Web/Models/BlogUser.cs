using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthFramework;

namespace Web.Models
{
    public class BlogUser : IUser
    {
        [Key]
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public List<BlogPost> BlogPosts { get; set; }

        [NotMapped] //  not stored in DB
        public string Password { get; set; }
    }
}
