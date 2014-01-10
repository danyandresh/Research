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
    public class BlogPostController : ControllerBase
    {
        public BlogPostController()
            : base()
        {
        }

        public BlogPostController(IRepository repository)
            : base(repository)
        {
        }

        public ActionResult Autocomplete(string term)
        {
            Func<BlogPost, bool> searchPosts = (p) => true;
            if (!string.IsNullOrEmpty(term))
            {
                searchPosts = (p) => p.Title.Contains(term);
            }

            return Json(db.Query<BlogPost>().Where(searchPosts).Take(10).Select(p => new { label = p.Title }), JsonRequestBehavior.AllowGet);
        }

        [OutputCache(CacheProfile = "reader")]
        public ActionResult Index(string search = null, int toPage = 1, int pageSize = 0)
        {
            if (pageSize == 0)
            {
                pageSize = Properties.Settings.Default.PageSize;
            }

            Func<BlogPost, bool> searchPosts = (p) => true;
            if (!string.IsNullOrEmpty(search))
            {
                searchPosts = (p) => p.Title.Contains(search) || p.Content.Contains(search);
            }

            var model = db.Query<BlogPost>().Where(searchPosts).AsQueryable().ToPagedList(pageSize, toPage);

            return Request.IsAjaxRequest() ? (ActionResult)PartialView("PostSummary", model) : (ActionResult)View(model);
        }

        public ActionResult Details(int id = 0)
        {
            BlogPost blogpost = db.Query<BlogPost>().First(e => e.Id == id);
            if (blogpost == null)
            {
                return HttpNotFound();
            }

            return View(blogpost);
        }

        public ActionResult Create()
        {
            return View();
        }

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

        public ActionResult Edit(int id = 0)
        {
            BlogPost blogpost = db.Query<BlogPost>().First(e => e.Id == id);
            if (blogpost == null)
            {
                return HttpNotFound();
            }
            return View(blogpost);
        }

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

        public ActionResult Delete(int id = 0)
        {
            BlogPost blogpost = db.Query<BlogPost>().First(e => e.Id == id);
            if (blogpost == null)
            {
                return HttpNotFound();
            }

            return View(blogpost);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BlogPost blogpost = db.Query<BlogPost>().First(e => e.Id == id);
            db.Remove(blogpost);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}