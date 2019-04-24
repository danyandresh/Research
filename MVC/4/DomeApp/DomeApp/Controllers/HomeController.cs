using DomeApp.Models;
using System.Web.Mvc;

namespace DomeApp.Controllers
{
    public class HomeController : ControllerBase
    {        
        public HomeController():base()
        {
        }

        public HomeController(IRepository repository)
            : base(repository)
        {
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
