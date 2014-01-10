using System.Web.Mvc;

namespace DomeApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("Index", "BlogPost");
        }

        public ActionResult About()
        {
            ViewBag.Message = "This is a MVC4 research and study project.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "github.com/danyandresh";

            return View();
        }
    }
}
