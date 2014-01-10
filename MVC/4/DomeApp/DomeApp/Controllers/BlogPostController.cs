using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DomeApp.Models;
using DomeApp.Extensions;
using System.Web.UI;

namespace DomeApp.Controllers
{
    public class BlogPostController : Controller
    {
        private IRepository db;

        public BlogPostController()
            : this(new DomeAppContext())
        {
        }

        public BlogPostController(IRepository repository)
        {
            db = repository;
        }

        //
        // GET: /BlgoPost/

        public ActionResult Autocomplete(string term)
        {
            Func<BlogPost, bool> searchPosts = (p) => true;
            if (!string.IsNullOrEmpty(term))
            {
                searchPosts = (p) => p.Title.Contains(term);
            }

            return Json(db.Query<BlogPost>().Where(searchPosts).Take(10).Select(p => new { label = p.Title }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Pages(string search)
        {
            var pageSize = 1;
            Func<BlogPost, bool> searchPosts = (p) => true;
            if (!string.IsNullOrEmpty(search))
            {
                searchPosts = (p) => p.Title.Contains(search);
            }

            var posts = db.Query<BlogPost>().Where(searchPosts);
            var pageCount = Math.Floor(1m + (decimal)(posts.Count() / pageSize));
            return Json(new { previous = false, total = pageCount, next = true }, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(CacheProfile = "reader")]
        public ActionResult Index(string search = null, int page = 1)
        {
            int pageSize = 6;
            Func<BlogPost, bool> searchPosts = (p) => true;
            if (!string.IsNullOrEmpty(search))
            {
                searchPosts = (p) => p.Title.Contains(search) || p.Content.Contains(search);
            }

            var model = db.Query<BlogPost>().Where(searchPosts).AsQueryable().ToPagedList(page, pageSize).ToList();

            return Request.IsAjaxRequest() ? (ActionResult)PartialView("PostSummary", model) : (ActionResult)View(model);
        }

        //
        // GET: /BlgoPost/Details/5

        public ActionResult Details(int id = 0)
        {
            BlogPost blogpost = db.Query<BlogPost>().First(e => e.Id == id);
            if (blogpost == null)
            {
                return HttpNotFound();
            }
            return View(blogpost);
        }

        //
        // GET: /BlgoPost/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /BlgoPost/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BlogPost blogpost)
        {
            if (ModelState.IsValid)
            {
                db.Add(blogpost);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(blogpost);
        }

        //
        // GET: /BlgoPost/Edit/5

        public ActionResult Edit(int id = 0)
        {
            BlogPost blogpost = db.Query<BlogPost>().First(e => e.Id == id);
            if (blogpost == null)
            {
                return HttpNotFound();
            }
            return View(blogpost);
        }

        //
        // POST: /BlgoPost/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BlogPost blogpost)
        {
            if (ModelState.IsValid)
            {
                //var updated=TryUpdateModel(blogpost);
                db.Update(blogpost);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(blogpost);
        }

        //
        // GET: /BlgoPost/Delete/5

        public ActionResult Delete(int id = 0)
        {
            BlogPost blogpost = db.Query<BlogPost>().First(e => e.Id == id);
            if (blogpost == null)
            {
                return HttpNotFound();
            }

            return View(blogpost);
        }

        //
        // POST: /BlgoPost/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BlogPost blogpost = db.Query<BlogPost>().First(e => e.Id == id);
            db.Remove(blogpost);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}