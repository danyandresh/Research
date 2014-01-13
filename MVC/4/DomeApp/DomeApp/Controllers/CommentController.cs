namespace DomeApp.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using DomeApp.Models;
    using DomeApp.Code.Paging;
    using System;

    public class CommentController : ControllerBase
    {
        public int CommentsPageSize
        {
            get { return Properties.Settings.Default.CommentsPageSize; }
        }

        public ActionResult Index(string search = null, int toPage = 1)
        {
            Func<Comment, bool> searchComments = c => true;

            if (!string.IsNullOrEmpty(search))
            {
                searchComments = c => c.Content.Contains(search);
            }

            var commentsQuery = db.Query<Comment>().Where(searchComments).AsQueryable();

            var model = commentsQuery.ToPagedList(CommentsPageSize, toPage);
            return Request.IsAjaxRequest() ? (ActionResult)PartialView(model) : (ActionResult)View(model);
        }

        public ActionResult CommentsByPost(int postId, int commentId = 0, int toPage = 1, bool toMostRecent = false)
        {
            var post = db.Query<BlogPost>().FirstOrDefault(p => p.Id == postId);

            if (post == null)
            {
                return HttpNotFound();
            }

            var pagedList = CommentsPageList(post);

            if (commentId != 0)
            {
                var comment = post.Comments.AsQueryable().FirstOrDefault(c => c.Id == commentId);
                if (comment == null)
                {
                    return HttpNotFound(string.Format("Comment with ID {0} not found", postId));
                }

                pagedList = pagedList.TakeFrom(c => c.Id == commentId);
            }

            pagedList.CurrentPage = toPage;

            if (toMostRecent)
            {
                pagedList.MoveLastPage();
            }

            var model = new Tuple<BlogPost, PagedList<Comment>>(post, pagedList);

            return PartialView(model);
        }

        [Authorize]
        public ActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(int postId, Comment comment, string returnUrl)
        {
            var post = db.Query<BlogPost>().FirstOrDefault(p => p.Id == postId);

            if (post == null)
            {
                return HttpNotFound(string.Format("Post {0} not found", postId));
            }

            if (ModelState.IsValid)
            {
                db.Add(comment);
                post.Comments.Add(comment);
                db.Update(post);
                db.SaveChanges();

                var model = new Tuple<BlogPost, PagedList<Comment>>(post, post.Comments.AsQueryable().ToPagedList(CommentsPageSize).MoveLastPage());

                //return PartialView("CommentsByPosdasasdst", model);
                ViewBag.ToMostRecent = true;
                return Redirect(returnUrl + "#comment-" + comment.Id);
            }

            return PartialView(comment);
        }

        [Authorize(Roles = "admin")]
        public ActionResult Edit(int id = 0)
        {
            var comment = db.Query<Comment>().FirstOrDefault(c => c.Id == id);
            if (comment == null)
            {
                return HttpNotFound();
            }

            return View(comment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [Authorize(Roles = "admin")]
        public ActionResult Edit(Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Update(comment);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = comment.Id });
            }
            return View(comment);
        }

        [Authorize]
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int id = 0)
        {
            var comment = db.Query<Comment>().FirstOrDefault(c => c.Id == id);
            if (comment == null)
            {
                return HttpNotFound();
            }

            return View(comment);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            var comment = db.Query<Comment>().FirstOrDefault(c => c.Id == id);
            db.Remove(comment);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        private PagedList<Comment> CommentsPageList(BlogPost post, int currentPage = 1)
        {
            return CommentsPageList((post.Comments ?? Enumerable.Empty<Comment>()).AsQueryable());

        }

        private PagedList<Comment> CommentsPageList(IQueryable<Comment> source)
        {
            var result = source.ToPagedList(CommentsPageSize);

            return result;
        }
    }
}