using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AuthFramework;
using EFCache;
using MySql.Data.MySqlClient;
using Z.EntityFramework.Plus;

namespace BenchmarkApp
{
    class Program
    {
        static void WithoutCaching()
        {
            var server = "localhost";
            var database = "test";
            var adminUsername = "root";
            var adminPassword = "";
            Configurator.ConfigureAdminAccount(server, database, adminUsername, adminPassword);

            var authUser = new AuthFramework.User()
            {
                Username = "user1234",
                Password = "heslo"
            };
            var contextProvider = new MysqlContextProvider();

            // returns posts with id less than 100
            using (var ctx = contextProvider.CreateContext<BlogContext>(authUser))
            {
                var query = ctx.Posts.Where(x => x.Id < 100).ToList();
            }
            
            // returns posts, which title starts with A
            using (var ctx = contextProvider.CreateContext<BlogContext>(authUser))
            {
                var query = ctx.Posts.Where(x => x.Title.StartsWith("A")).ToList();
            }
            
            // returns posts, which body text starts with E
            using (var ctx = contextProvider.CreateContext<BlogContext>(authUser))
            {
                var query = ctx.Posts.Where(x => x.Body.StartsWith("E")).ToList();
            }
            
            // returns posts, which has not been deleted
            using (var ctx = contextProvider.CreateContext<BlogContext>(authUser))
            {
                var query = ctx.Posts.Where(x => x.Deleted == false).ToList();
            }

            // returns users with id less than 10
            using (var ctx = contextProvider.CreateContext<BlogContext>(authUser))
            {
                var query = ctx.Users.Where(x => x.Id < 10).ToList();
            }

            // returns users, whose email contains substring: example
            using (var ctx = contextProvider.CreateContext<BlogContext>(authUser))
            {
                var query = ctx.Users.Where(x => x.Email.Contains("example")).ToList();
            }

            // returns users, whose username starts with U
            using (var ctx = contextProvider.CreateContext<BlogContext>(authUser))
            {
                var query = ctx.Users.Where(x => x.UserName.StartsWith("U")).ToList();
            }
            
            //returns active user, who has the longest lasting profile
            using (var ctx = contextProvider.CreateContext<BlogContext>(authUser))
            {
                var query = ctx.Users.Where(x => x.Active == true).OrderBy(x => x.Id).First();
            }
            
            // returns percentage of active users
            using(var ctx = contextProvider.CreateContext<BlogContext>(authUser))
            {
                double query = (double)ctx.Users.Where(user => user.Active == true).Count() / ctx.Users.Count();
            }

        }

        static void WithCaching()
        {
            var server = "localhost";
            var database = "test";
            var adminUsername = "root";
            var adminPassword = "";
            Configurator.ConfigureAdminAccount(server, database, adminUsername, adminPassword);

            var authUser = new AuthFramework.User()
            {
                Username = "user1234",
                Password = "heslo"
            };

            var contextProvider = new MysqlContextProvider();

            // returns posts with id less than 100
            using (var ctx = contextProvider.CreateContext<BlogContext>(authUser))
            {
                var query = ctx.Posts.FromCache().Where(x => x.Id < 100).ToList();
            }

            // returns posts, which title starts with A
            using (var ctx = contextProvider.CreateContext<BlogContext>(authUser))
            {
                var query = ctx.Posts.FromCache().Where(x => x.Title.StartsWith("A")).ToList();
            }

            // returns posts, which body text starts with E
            using (var ctx = contextProvider.CreateContext<BlogContext>(authUser))
            {
                var query = ctx.Posts.FromCache().Where(x => x.Body.StartsWith("E")).ToList();
            }

            // returns posts, which has not been deleted
            using (var ctx = contextProvider.CreateContext<BlogContext>(authUser))
            {
                var query = ctx.Posts.FromCache().Where(x => x.Deleted == false).ToList();
            }

            // returns users with id less than 10
            using (var ctx = contextProvider.CreateContext<BlogContext>(authUser))
            {
                var query = ctx.Users.FromCache().Where(x => x.Id < 10).ToList();
            }

            // returns users, whose email contains substring: example
            using (var ctx = contextProvider.CreateContext<BlogContext>(authUser))
            {
                var query = ctx.Users.FromCache().Where(x => x.Email.Contains("example")).ToList();
            }

            // returns users, whose username starts with U
            using (var ctx = contextProvider.CreateContext<BlogContext>(authUser))
            {
                var query = ctx.Users.FromCache().Where(x => x.UserName.StartsWith("U")).ToList();
            }

            //returns active user, who has the longest lasting profile
            using (var ctx = contextProvider.CreateContext<BlogContext>(authUser))
            {
                var query = ctx.Users.FromCache().Where(x => x.Active == true).OrderBy(x => x.Id).First();
            }

            // returns percentage of active users
            using (var ctx = contextProvider.CreateContext<BlogContext>(authUser))
            {
                double query = (double)ctx.Users.FromCache().Where(user => user.Active == true).Count() / ctx.Users.FromCache().Count();
            }

        }


        static void GenericAccount()
        {         
            string connectionString = "server=localhost;database=test50;uid=root";
            var user1234 = new User()
            {
                UserName = "user1234",
                Email = "user1234@muni.cz",
                Password = "heslo",
                Created = DateTime.Now
            };

            var post1 = new Post()
            {
                Title =
                    "Neque porro quisquam est qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit...",
                Body =
                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras rutrum elit posuere finibus ornare. Quisque non arcu turpis. Curabitur a volutpat ante. Etiam quis ipsum nec sem efficitur vestibulum. Vivamus a velit vehicula, eleifend turpis et, mollis felis. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Vivamus ligula dui, elementum id volutpat eget, convallis eu mauris. Pellentesque risus felis, egestas eget porta non, scelerisque vel leo. Vestibulum hendrerit sapien sed tellus placerat aliquam. Mauris eu eros a tortor finibus viverra bibendum ut nulla. Phasellus pellentesque, leo ac consectetur pulvinar, dui enim auctor libero, vitae fringilla ex turpis in ligula. Nam et venenatis leo, sed condimentum nulla. Vestibulum luctus sapien nec erat tempor viverra. Donec id magna vitae nibh placerat pharetra. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Integer euismod fringilla vestibulum.",
                User = user1234,
                Published = DateTime.Now
            };

            var post2 = new Post()
            {
                Title =
                    "Aliquam dignissim eget",
                Body =
                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent volutpat pretium turpis sit amet ornare. Donec a hendrerit purus, id sagittis felis. Phasellus at dui turpis. Nam quis lectus ipsum. Etiam laoreet tortor eget imperdiet suscipit. Donec vestibulum enim vitae nibh sollicitudin, ut lacinia justo commodo. Donec vulputate sapien at sem bibendum, vel auctor dui vehicula. Maecenas consequat massa ac turpis accumsan finibus. Sed blandit accumsan turpis, in ornare mauris. Quisque eu lacinia orci. Suspendisse vitae risus iaculis, porta urna non, cursus lorem. Morbi nec ipsum ut sapien pretium commodo.Aliquam dignissim eget risus sed bibendum.Ut ligula quam,facilisis quis sagittis sed,euismod a ligula.Praesent ac nibh at mauris vehicula aliquet.Duis pretium leo est,vel venenatis eros finibus sit amet.Mauris massa enim,dignissim eget neque eget,ornare accumsan enim.Sed id lacus ut libero luctus consequat suscipit nec diam.Maecenas mollis vehicula faucibus.Aenean euismod,elit non vehicula porttitor,metus lacus fermentum sapien,elementum gravida ligula lorem facilisis quam.Integer luctus,mi quis tempor tempus,ex purus volutpat dolor,sed dapibus eros turpis sed nunc.Aenean ut interdum velit.Mauris ullamcorper justo ac maximus aliquam.Nullam euismod leo id vulputate varius.Curabitur id faucibus sapien.Pellentesque tincidunt ligula quam,posuere pulvinar ligula rhoncus eget.Pellentesque magna neque,hendrerit eu suscipit et,malesuada aliquam tortor.Vestibulum id maximus odio.",
                User = user1234,
                Published = DateTime.Now
            };

            var post3 = new Post()
            {
                Title =
                    "Praesent volutpat pretium turpis",
                Body =
                    "Vivamus auctor, felis ultrices luctus bibendum, nisi ligula laoreet nisi, vitae pretium sapien libero sit amet leo. In feugiat magna in dolor elementum faucibus. Aenean non velit nec dolor tincidunt commodo a a urna. Duis venenatis nulla vel consequat dictum. Duis eu massa vulputate, dictum nisl non, tempor purus. Aliquam sed ligula nec nisi dapibus elementum. Morbi et efficitur nulla. Cras elit quam, porttitor vitae leo eu, lacinia euismod lorem. Vestibulum pellentesque ante et dapibus tempor. Sed ac lectus nunc. Suspendisse aliquam turpis ut velit gravida tristique.Nullam eget lacus vel sem dapibus ornare quis eget tortor.Ut suscipit tincidunt enim,ut placerat odio pharetra at.Quisque rutrum est eget massa tincidunt tempus.Ut a odio faucibus,placerat velit in,finibus magna.Nam malesuada lacus libero,quis molestie nunc rutrum vel.Vivamus a ipsum vitae leo accumsan fringilla.In tristique nunc nec augue mollis eleifend.Aliquam enim urna,accumsan sit amet fermentum nec,bibendum a libero.Nam at tempus sapien.Pellentesque pharetra ullamcorper dapibus.Vivamus libero massa,tincidunt eget consequat sit amet,semper porta quam.Praesent vehicula interdum velit eu volutpat.Nam lacus justo,tempus in tortor vel,euismod luctus sapien.Phasellus tempus nunc purus,nec faucibus erat tincidunt id.Suspendisse augue metus,dictum id ligula nec,maximus varius sapien.",
                User = user1234,
                Published = DateTime.Now
            };


            var connection = new MySqlConnection(connectionString);

            using (var blContext = new BlogContext(connection, false))
            {
                blContext.Users.Add(user1234);
                blContext.SaveChanges();
            }               

            using (var blContext = new BlogContext(connection, false))
            {
                blContext.Posts.Add(post1);
                blContext.SaveChanges();
            }                

            using (var blContext = new BlogContext(connection, false))
            {
                blContext.Posts.Add(post2);
                blContext.SaveChanges();
            }
;
            using (var blContext = new BlogContext(connection, false))
            {
                blContext.Posts.Add(post3);
                blContext.SaveChanges();
            }

            // returns titles and comments count of 5 most commented posts
            using (var blContext = new BlogContext(connection, false))
            {
                var query = blContext.Posts.GroupJoin(blContext.Comments,
                        post => post,
                        cmnt => cmnt.Post,
                        (post, cmnt) => new { post.Title, cmnt })
                    .OrderByDescending(x => x.cmnt.Count())
                    .Take(5)
                    .Select((x) => new { x.Title, Count = x.cmnt.Count() })
                    .ToList();
            }

            // returns percentage of active users
            using (var blContext = new BlogContext(connection, false))
            {                
                double percentage = (double)blContext.Users.Where(user => user.Active == true).Count() / blContext.Users.Count();
            }

            // changes email of user
            using (var blContext = new BlogContext(connection, false))
            {                
                var usr = blContext.Users.Where(x => x.Email == user1234.Email).First();
                usr.Email = "user1234@fi.muni.cz";
                blContext.SaveChanges();
            }

            //changes content of posts posted by user1234
            using (var blContext = new BlogContext(connection, false))
            {
                var postList = blContext.Posts.Where(x => x.User.UserName == user1234.UserName)
                    .ToList();

                foreach (var p in postList)
                {
                    p.Body = p.Body + " written by user1234";
                    blContext.SaveChanges();
                }
            }

            //add comment to last post
            using (var blContext = new BlogContext(connection, false))
            {  
                var lastPost = blContext.Posts.OrderByDescending(x => x.Published).First();

                var comment = new Comment()
                {
                    CommentDate = DateTime.Now,
                    CommentText = "This is my comment",
                    Post = lastPost,
                    User = user1234
                };

                blContext.Comments.Add(comment);
                blContext.SaveChanges();
            }

            //returns active user, who has the longest lasting profile
            using (var blContext = new BlogContext(connection, false))
            {
                var longestLastingProfileUser =
                    blContext.Users.Where(x => x.Active == true).OrderBy(x => x.Created).First();
            }

            //returns, how much posts in average was posted by users, who are no longer active
            using (var blContext = new BlogContext(connection, false))
            {          
                double postsCountByInactiveUsers =
                    blContext.Users.Where(x => x.Active == false).GroupJoin(
                            blContext.Posts,
                            usr => usr.Id,
                            post => post.Id,
                            (usr, post) => post)
                        .Average(yz => yz.Count());
            }

            // returns list of tuples contains username and post titles
            using (var blContext = new BlogContext(connection, false))
            {
                blContext.Database.CommandTimeout = 180;                
                var usersAndTheriPostsTitles =
                    blContext.Users.GroupJoin(
                            blContext.Posts,
                            usr => usr.Id,
                            post => post.User.Id,
                            (usr, post) => new { usr.UserName, post })
                        .AsEnumerable()
                        .Select(x => new Tuple<string, List<string>>(x.UserName, x.post.Select(s => s.Title).ToList())).ToList();
            }
        }

        static void NonGenericAccount()
        {
            var server = "localhost";
            var database = "test";
            var adminUsername = "root";
            var adminPassword = "";
            Configurator.ConfigureAdminAccount(server, database, adminUsername, adminPassword);

            var authUser = new AuthFramework.User()
            {
                Username = "user1234",
                Password = "heslo"
            };

            var user1234 = new User()
            {
                UserName = "user1234",
                Email = "user1234@muni.cz",
                Password = "heslo",
                Created = DateTime.Now
            };

            var post1 = new Post()
            {
                Title =
                  "Neque porro quisquam est qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit...",
                Body =
                  "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras rutrum elit posuere finibus ornare. Quisque non arcu turpis. Curabitur a volutpat ante. Etiam quis ipsum nec sem efficitur vestibulum. Vivamus a velit vehicula, eleifend turpis et, mollis felis. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Vivamus ligula dui, elementum id volutpat eget, convallis eu mauris. Pellentesque risus felis, egestas eget porta non, scelerisque vel leo. Vestibulum hendrerit sapien sed tellus placerat aliquam. Mauris eu eros a tortor finibus viverra bibendum ut nulla. Phasellus pellentesque, leo ac consectetur pulvinar, dui enim auctor libero, vitae fringilla ex turpis in ligula. Nam et venenatis leo, sed condimentum nulla. Vestibulum luctus sapien nec erat tempor viverra. Donec id magna vitae nibh placerat pharetra. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Integer euismod fringilla vestibulum.",
                User = user1234,
                Published = DateTime.Now
            };

            var post2 = new Post()
            {
                Title =
                    "Aliquam dignissim eget",
                Body =
                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent volutpat pretium turpis sit amet ornare. Donec a hendrerit purus, id sagittis felis. Phasellus at dui turpis. Nam quis lectus ipsum. Etiam laoreet tortor eget imperdiet suscipit. Donec vestibulum enim vitae nibh sollicitudin, ut lacinia justo commodo. Donec vulputate sapien at sem bibendum, vel auctor dui vehicula. Maecenas consequat massa ac turpis accumsan finibus. Sed blandit accumsan turpis, in ornare mauris. Quisque eu lacinia orci. Suspendisse vitae risus iaculis, porta urna non, cursus lorem. Morbi nec ipsum ut sapien pretium commodo.Aliquam dignissim eget risus sed bibendum.Ut ligula quam,facilisis quis sagittis sed,euismod a ligula.Praesent ac nibh at mauris vehicula aliquet.Duis pretium leo est,vel venenatis eros finibus sit amet.Mauris massa enim,dignissim eget neque eget,ornare accumsan enim.Sed id lacus ut libero luctus consequat suscipit nec diam.Maecenas mollis vehicula faucibus.Aenean euismod,elit non vehicula porttitor,metus lacus fermentum sapien,elementum gravida ligula lorem facilisis quam.Integer luctus,mi quis tempor tempus,ex purus volutpat dolor,sed dapibus eros turpis sed nunc.Aenean ut interdum velit.Mauris ullamcorper justo ac maximus aliquam.Nullam euismod leo id vulputate varius.Curabitur id faucibus sapien.Pellentesque tincidunt ligula quam,posuere pulvinar ligula rhoncus eget.Pellentesque magna neque,hendrerit eu suscipit et,malesuada aliquam tortor.Vestibulum id maximus odio.",
                User = user1234,
                Published = DateTime.Now
            };

            var post3 = new Post()
            {
                Title =
                    "Praesent volutpat pretium turpis",
                Body =
                    "Vivamus auctor, felis ultrices luctus bibendum, nisi ligula laoreet nisi, vitae pretium sapien libero sit amet leo. In feugiat magna in dolor elementum faucibus. Aenean non velit nec dolor tincidunt commodo a a urna. Duis venenatis nulla vel consequat dictum. Duis eu massa vulputate, dictum nisl non, tempor purus. Aliquam sed ligula nec nisi dapibus elementum. Morbi et efficitur nulla. Cras elit quam, porttitor vitae leo eu, lacinia euismod lorem. Vestibulum pellentesque ante et dapibus tempor. Sed ac lectus nunc. Suspendisse aliquam turpis ut velit gravida tristique.Nullam eget lacus vel sem dapibus ornare quis eget tortor.Ut suscipit tincidunt enim,ut placerat odio pharetra at.Quisque rutrum est eget massa tincidunt tempus.Ut a odio faucibus,placerat velit in,finibus magna.Nam malesuada lacus libero,quis molestie nunc rutrum vel.Vivamus a ipsum vitae leo accumsan fringilla.In tristique nunc nec augue mollis eleifend.Aliquam enim urna,accumsan sit amet fermentum nec,bibendum a libero.Nam at tempus sapien.Pellentesque pharetra ullamcorper dapibus.Vivamus libero massa,tincidunt eget consequat sit amet,semper porta quam.Praesent vehicula interdum velit eu volutpat.Nam lacus justo,tempus in tortor vel,euismod luctus sapien.Phasellus tempus nunc purus,nec faucibus erat tincidunt id.Suspendisse augue metus,dictum id ligula nec,maximus varius sapien.",
                User = user1234,
                Published = DateTime.Now
            };

            var contextProvider = new MysqlContextProvider();

            using (var blContext = contextProvider.CreateContext<BlogContext>(authUser))
            {
                blContext.Users.Add(user1234);
                blContext.SaveChanges();
            }

            using (var blContext = contextProvider.CreateContext<BlogContext>(authUser))
            {
                blContext.Posts.Add(post1);
                blContext.SaveChanges();
                
            }

            using (var blContext = contextProvider.CreateContext<BlogContext>(authUser))
            {
                blContext.Posts.Add(post2);
                blContext.SaveChanges();
            }

            using (var blContext = contextProvider.CreateContext<BlogContext>(authUser))
            {
                blContext.Posts.Add(post3);
                blContext.SaveChanges();
            }

            // returns titles and comments count of 5 most commented posts
            using (var blContext = contextProvider.CreateContext<BlogContext>(authUser))
            {                
                var query = blContext.Posts.GroupJoin(blContext.Comments,
                        post => post,
                        cmnt => cmnt.Post,
                        (post, cmnt) => new { post.Title, cmnt })
                    .OrderByDescending(x => x.cmnt.Count())
                    .Take(5)
                    .Select((x) => new { x.Title, Count = x.cmnt.Count() })
                    .ToList();
            }

            // returns percentage of active users
            using (var blContext = contextProvider.CreateContext<BlogContext>(authUser))
            {
                double percentage = (double)blContext.Users.Where(user => user.Active == true).Count() / blContext.Users.Count();
            }

            // changes email of user
            using (var blContext = contextProvider.CreateContext<BlogContext>(authUser))
            {                
                var usr = blContext.Users.Where(x => x.Email == user1234.Email).First();
                usr.Email = "user1234@fi.muni.cz";
                blContext.SaveChanges();
            }

            //changes content of posts posted by user1234
            using (var blContext = contextProvider.CreateContext<BlogContext>(authUser))
            {
                var postList = blContext.Posts.Where(x => x.User.UserName == user1234.UserName)
                    .ToList();

                foreach (var p in postList)
                {
                    p.Body = p.Body + " written by user1234";
                    blContext.SaveChanges();
                }
            }

            //add comment to last post
            using (var blContext = contextProvider.CreateContext<BlogContext>(authUser))
            {
                var lastPost = blContext.Posts.OrderByDescending(x => x.Published).First();

                var comment = new Comment()
                {
                    CommentDate = DateTime.Now,
                    CommentText = "This is my comment",
                    Post = lastPost,
                    User = user1234
                };

                blContext.Comments.Add(comment);
                blContext.SaveChanges();
            }

            //returns active user, who has the longest lasting profile
            using (var blContext = contextProvider.CreateContext<BlogContext>(authUser))
            {                
                var longestLastingProfileUser =
                    blContext.Users.Where(x => x.Active == true).OrderBy(x => x.Created).First();                
            }

            //returns, how much posts in average was posted by users, who are no longer active
            using (var blContext = contextProvider.CreateContext<BlogContext>(authUser))
            {
                double postsCountByInactiveUsers =
                    blContext.Users.Where(x => x.Active == false).GroupJoin(
                            blContext.Posts,
                            usr => usr.Id,
                            post => post.Id,
                            (usr, post) => post)
                        .Average(yz => yz.Count());
            }

            // returns list of tuples contains username and post titles
            using (var blContext = contextProvider.CreateContext<BlogContext>(authUser))
            {
                blContext.Database.CommandTimeout = 180;
                var usersAndTheriPostsTitles =
                    blContext.Users.GroupJoin(
                            blContext.Posts,
                            usr => usr.Id,
                            post => post.User.Id,
                            (usr, post) => new { usr.UserName, post })
                        .AsEnumerable()
                        .Select(x => new Tuple<string, List<string>>(x.UserName, x.post.Select(s => s.Title).ToList())).ToList();
            }
        }

        static double TimeMethod(Action methodToTime)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            methodToTime();
            stopwatch.Stop();
            return stopwatch.Elapsed.TotalSeconds;
        }

        static void Main(string[] args)
        {

            Console.WriteLine("With caching: {0} sec", TimeMethod(WithCaching));
            //Console.WriteLine("Without caching: {0} sec", TimeMethod(WithoutCaching));

            //Console.WriteLine("Generic account: {0} sec", TimeMethod(GenericAccount));
            //Console.WriteLine("NonGeneric account: {0} sec", TimeMethod(NonGenericAccount));
            System.Console.Read();
        }
    }
}
