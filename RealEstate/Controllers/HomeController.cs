using MongoDB.Bson;
using MongoDB.Driver;
using RealEstate.App_Start;
using System.Web.Mvc;

namespace RealEstate.Controllers
{
    public class HomeController : Controller
    {

        private RealEstateContext _context = new RealEstateContext();

        public ActionResult Index()
        {
            var dbStatsCommand = new CommandDocument("dbStats", 1);
            var dbStatsResult = _context.Database.RunCommand<BsonDocument>(dbStatsCommand);

            var buildInfoCommand = new CommandDocument("buildinfo", 1);
            var result = _context.Database.RunCommand<BsonDocument>(buildInfoCommand);
            return Json(result.ToJson(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}