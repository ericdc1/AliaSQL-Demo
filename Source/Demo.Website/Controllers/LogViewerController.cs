using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NLog;
using NLog.Targets;
using NLog.Targets.Wrappers;

namespace Demo.Website.Controllers
{
    public class LogViewerController : Controller
    {
        public ActionResult Index()
        {
            return View(GetLogDirectory());
        }


        public ActionResult ViewLog(string filename, string foldername)
        {
            var directory = GetLogDirectory();
            var loggerFolder = directory.GetDirectories().FirstOrDefault(i => i.Name == foldername);
            if (loggerFolder != null)
            {
                var firstMatch = loggerFolder.GetFiles("*.log").FirstOrDefault(i => i.Name == filename);
                if (firstMatch != null)
                    return Content(System.IO.File.ReadAllText(firstMatch.FullName));
            }
            return Content("File does not exist");
        }

        private DirectoryInfo GetLogDirectory()
        {
            //get the directory a logger would be writing to.
            var fileTarget = ((WrapperTargetBase)LogManager.Configuration.FindTargetByName("filelog")).WrappedTarget as FileTarget;
            var logEventInfo = new LogEventInfo { TimeStamp = DateTime.Now };
            var fileName = fileTarget.FileName.Render(logEventInfo);
            return new System.IO.FileInfo(fileName).Directory;
        }

    }
}