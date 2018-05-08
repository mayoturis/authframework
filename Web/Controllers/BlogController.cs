using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AuthFramework;
using Web.Models;

namespace Web.Controllers
{
    public class BlogController : Controller
    {
        // GET: Blog
        public ActionResult Index()
        {
            var contextProvider = new MysqlContextProvider();
            using (var context = contextProvider.CreateContext<BlogContext>())
            {
                var blogs = context.BlogPosts.ToList();
                return View(blogs);
            }
            return View("~/Views/Shared/Error.cshtml");
        }

        // GET: Blog
        public ActionResult IndexBlogsOfUser()
        {
            BlogUser currentUser = (BlogUser)Session["user"];
            if (currentUser == null)
            {
                return View("~/Views/Account/NotLogged.cshtml");
            }
            var contextProvider = new MysqlContextProvider();
            using (var context = contextProvider.CreateContext<BlogContext>()) // explicitly authenticated by user2
            {
                var blogs = context.BlogPosts.Where(x => x.CreatedBy.Id.Equals(currentUser.Id)).ToList();
                return View(blogs);
            }
            return View("~/Views/Shared/Error.cshtml");
        }



        // GET: Blog/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Blog/Create
        [HttpPost]
        public ActionResult Create(BlogPost blog)
        {
            BlogUser loggedUser = (BlogUser)Session["user"];
            if (loggedUser == null)
            {
                return View("~/Views/Account/NotLogged.cshtml");
            }
            if (ModelState.IsValid)
            {
                var authenticator = new Authenticator();
                var contextProvider = new MysqlContextProvider();

                    using (var context = contextProvider.CreateContext<BlogContext>()) // authenticated by user1 as default
                    {
                    // Table in DB have to be already created at this point. It's better to do it by hand, then to use strategies,
                    // because they drop the whole DB which we don't want since users already have access rights set to the given DB
                    // context.BlogUsers.Attach(loggedUser); // we need to attach user1 because otherwise it is detached entity (EF) doesn't know about it and new one would be created
                    context.BlogUsers.Attach(loggedUser);
                    context.BlogPosts.Add(new BlogPost()
                        {
                            Title = blog.Title,
                            Text = blog.Text,
                            CreatedBy = loggedUser,
                            CreatedOn = DateTime.Now,
                            ModifiedOn = DateTime.Now
                        });
                        context.SaveChanges();
                    }

                    return RedirectToAction("IndexBlogsOfUser");
            }
             
            return View("Error");
        }

        // GET: Blog/Details/5
        public ActionResult Details(int id)
        {
            var authenticator = new Authenticator();
            var contextProvider = new MysqlContextProvider();
            using (var context = contextProvider.CreateContext<BlogContext>()) // authenticated by user1 as default
            {
                // Table in DB have to be already created at this point. It's better to do it by hand, then to use strategies,
                BlogPost blog = context.BlogPosts.Find(id);
                return View(blog);
            }
            return View();
        }

        // GET: Blog/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogUser currentUser = (BlogUser)Session["user"];
            if (currentUser == null)
            {
                return View("~/Views/Account/NotLogged.cshtml");
            }
            var contextProvider = new MysqlContextProvider();
            using (var context = contextProvider.CreateContext<BlogContext>()) // explicitly authenticated by user2
            {
                var blog = context.BlogPosts.Find(id);
                return View(blog);
            }
            return View();
        }
 

        // POST: Blog/Edit/5
        [HttpPost]
        public ActionResult Edit(BlogPost blogPost)
        {
            BlogUser loggedUser = (BlogUser)Session["user"];
            if (loggedUser == null)
            {
                return View("~/Views/Account/NotLogged.cshtml");
            }
            if (ModelState.IsValid)
            {
                var authenticator = new Authenticator();
                var contextProvider = new MysqlContextProvider();
                using (var context = contextProvider.CreateContext<BlogContext>()) // authenticated by user1 as default
                {
                    // Table in DB have to be already created at this point. It's better to do it by hand, then to use strategies,
                    context.BlogUsers.Attach(loggedUser);
                    BlogPost updateBlog = context.BlogPosts.Find(blogPost.Id);
                    updateBlog.Title = blogPost.Title;
                    updateBlog.Text = blogPost.Text;
                    updateBlog.ModifiedOn = DateTime.Now;
                    context.BlogPosts.AddOrUpdate(updateBlog);

                    context.SaveChanges();
                }

                return RedirectToAction("IndexBlogsOfUser");
            }

            return View("Error");
        }

        // POST: Blog/Delete
        //[HttpPost]
        public ActionResult Delete(int id)
        {
            BlogUser currentUser = (BlogUser)Session["user"];
            var authenticator = new Authenticator();
            var contextProvider = new MysqlContextProvider();
            BlogUser loggedUser = (BlogUser)Session["user"];
            using (var context = contextProvider.CreateContext<BlogContext>(currentUser))
            {
                context.BlogPosts.Remove(context.BlogPosts.Find(id));
                context.SaveChanges();
            };
            return RedirectToAction("IndexBlogsOfUser");

        }
        
    }
}
