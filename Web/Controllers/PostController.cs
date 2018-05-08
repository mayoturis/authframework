using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class PostController : Controller
    {
        // GET: Post
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Post()
        {
            /*
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
            authenticator.SetUser(user2);*/

            return View();
        }



}
}