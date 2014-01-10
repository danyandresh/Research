using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DomeApp.Controllers
{
    public abstract class ControllerBase : Controller
    {
        protected ActionResult RedirectHome()
        {
            return RedirectToAction("", "", null);
        }
    }
}