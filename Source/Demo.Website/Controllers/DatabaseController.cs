using System;
using System.Configuration;
using System.Web.Mvc;
using AliaSQL.Core;
using NLog;

namespace Demo.Website.Controllers
{
    public class DatabaseController : Controller
    {
        private static Logger logger = LogManager.GetLogger("DatabaseMigrations");

        // GET: Database
        public ActionResult Index()
        {
            ViewBag.pendingchanges = new DbUpdater().PendingChanges(ConfigurationManager.ConnectionStrings["DemoConnectionString"].ConnectionString);
            ViewBag.pendingtestdata = new DbUpdater().PendingTestData(ConfigurationManager.ConnectionStrings["DemoConnectionString"].ConnectionString);
            return View();
        }

        public ActionResult Update()
        {
            var result = new DbUpdater().UpdateDatabase(ConfigurationManager.ConnectionStrings["DemoConnectionString"].ConnectionString);
            if (result.Success)
            {
                logger.Info(result.Result);
                return RedirectToAction("index");
            }
            logger.Error(result.Result);
            return Content(result.Result.Replace(Environment.NewLine, "<br>"));
        }

        public ActionResult UpdateTestData()
        {
            var result = new DbUpdater().UpdateDatabase(ConfigurationManager.ConnectionStrings["DemoConnectionString"].ConnectionString, RequestedDatabaseAction.TestData);
            if (result.Success)
            {
                logger.Info(result.Result);
                return RedirectToAction("index");
            }
            logger.Error(result.Result);
            return Content(result.Result.Replace(Environment.NewLine, "<br>"));
        }
    }
}