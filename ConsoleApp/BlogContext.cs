using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class BlogContext : DbContext
    {

        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<BlogUser> BlogUsers { get; set; }

        // has to have constructor with one parameter so that connection string can be injected by framework
        public BlogContext(string connectionString) : base(connectionString)
        {

        }
    }
}
