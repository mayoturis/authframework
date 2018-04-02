using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthFramework;
using MySql.Data.MySqlClient;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // configures admin account which will be used for registrations
            // all values can stay the same for you except database. If you create DB with different name you need to change it
            var server = "localhost"; 
            var database = "authdb";
            var adminUsername = "root";
            var adminPassword = "";
            Configurator.ConfigureAdminAccount(server, database, adminUsername, adminPassword);

            // ------------ some generic example -------------

            var user = new User()
            {
                Username = "Marek",
                Password = "password"
            };

            var authenticator = new Authenticator();
            authenticator.Register(user);

            // should be true
            Console.Out.WriteLine(authenticator.CanAuthenticate(user));

            authenticator.Delete(user.Username);

            // should be false
            Console.Out.WriteLine(authenticator.CanAuthenticate(user));

            // ---------------- simple blog example ------------

            var user1 = new BlogUser()
            {
                Username = "User1",
                Password = "password1",
                FullName = "John Doe"
            };
            var user2 = new BlogUser()
            {
                Username = "User2",
                Password = "password2",
                FullName = "Jane Doe"
            };

            authenticator.Register(user1); // don't run this twice, it will throw exception that same user already exists
            authenticator.Register(user2);

            var contextProvider = new MysqlContextProvider();
            // we have to add blog user after authenticator.Register() because we need that user to authenticate the transaction
            using (var context = contextProvider.CreateContext<BlogContext>(user1)) // authenticated by user1
            {
                context.BlogUsers.Add(user1);
                context.SaveChanges();
            }

            using (var context = contextProvider.CreateContext<BlogContext>(user2)) // authenticated by user2
            {
                context.BlogUsers.Add(user2);
                context.SaveChanges();
            }

            // this will set user1 as the default for all following transaction authentications
            if (!authenticator.Authenticate(user1))
                throw new InvalidOperationException("This should never happen because user was registered");

            using (var context = contextProvider.CreateContext<BlogContext>()) // authenticated by user1 as default
            {
                // Table in DB have to be already created at this point. It's better to do it by hand, then to use strategies,
                // because they drop the whole DB which we don't want since users already have access rights set to the given DB
                context.BlogUsers.Attach(user1); // we need to attach user1 because otherwise it is detached entity (EF) doesn't know about it and new one would be created
                context.BlogPosts.Add(new BlogPost()
                {
                    Title = "Some title",
                    BlogUser = user1
                });
                context.SaveChanges();
            }

            using (var context = contextProvider.CreateContext<BlogContext>(user2)) // explicitly authenticated by user2
            {
                foreach (var post in context.BlogPosts.Include("bloguser"))
                {
                    Console.Out.WriteLine($"{post.BlogUser.FullName} : {post.Title}");
                }
            }

            // from now on, user2 will be used as default for authentication
            authenticator.SetUser(user2);
        }
    }
}
