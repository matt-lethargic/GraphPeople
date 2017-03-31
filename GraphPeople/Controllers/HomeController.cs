using System.Web.Mvc;

namespace GraphPeople.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}