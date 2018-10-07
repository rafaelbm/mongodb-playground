using MongoDB.Bson;
using MongoDB.Driver;
using RealEstate.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RealEstate.Controllers
{
    public class HomeController : Controller
    {
        private IMongoDatabase database;

        public HomeController()
        {
            var client = new MongoClient(Settings.Default.RealEstateConnectionString);
            database = client.GetDatabase(Settings.Default.RealEstateDatabaseName);
        }

        public ActionResult Index()
        {
            var dbStatsCommand = new CommandDocument("dbStats", 1);
            var dbStatsResult = database.RunCommand<BsonDocument>(dbStatsCommand);

            var buildInfoCommand = new CommandDocument("buildinfo", 1);
            var result = database.RunCommand<BsonDocument>(buildInfoCommand);
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