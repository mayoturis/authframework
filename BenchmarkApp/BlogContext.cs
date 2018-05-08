using MySql.Data.MySqlClient;
using System;
using System.Data.Common;
using System.Data.Entity;
using EFCache;
using MySql.Data.Entity;

namespace BenchmarkApp
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    //[DbConfigurationType(typeof(BlogConfiguration))]
    class BlogContext : DbContext
    {        
        public BlogContext() : base()
        {

        }

        public BlogContext(string connectionString) : base(connectionString)
        {

        }

        public BlogContext(DbConnection existingConnection, bool contextOwnConnection) : base(existingConnection, contextOwnConnection)
        {
            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<BlogContext>());
        }        

        public DbSet<Post> Posts { get; set; }        
        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }

}