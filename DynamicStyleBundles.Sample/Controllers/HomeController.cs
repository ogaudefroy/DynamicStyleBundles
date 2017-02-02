namespace DynamicStyleBundles.Sample.Controllers
{
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            this.ViewData["Msg"] = "Hello world";
            return View();
        }
    }
}