using DomeApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DomeApp.Controllers
{
    public abstract class ControllerBase : Controller
    {
        protected IRepository db;

        protected ControllerBase()
            : this(new DomeAppContext())
        {
        }

        protected ControllerBase(IRepository repository)
        {
            db = repository;
        }

        protected ActionResult RedirectHome()
        {
            return RedirectToAction("", "", null);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        protected UserProfile GetCurrentUser()
        {
            if (!User.Identity.IsAuthenticated)
            {
                throw new HttpException((int)HttpStatusCode.Forbidden, "Login first");
            }

            return db.Query<UserProfile>().First(u => u.UserName == User.Identity.Name);
        }
    }
}