using System;
using System.Net;
using System.Web.Mvc;
using StackExchange.Exceptional;

namespace Demo.Website.Controllers
{
    public class ErrorController : Controller
    {

        public ActionResult Index(Exception error, int status = 404)
        {
            Response.StatusCode = status;
            return View(new ErrorInfo { StatusCode = status, Exception = error });
        }


        public ActionResult Exceptions()
        {
            var context = System.Web.HttpContext.Current;
            var page = new HandlerFactory().GetHandler(context, Request.RequestType, Request.Url.ToString(), Request.PathInfo);
            page.ProcessRequest(context);
            return null;
        }

        public ActionResult ErrorTestPage()
        {
            throw new NotImplementedException("I AM IMPLEMENTED, I WAS BORN TO THROW ERRORS!");
        }

        public class ErrorInfo
        {
            public int StatusCode { get; set; }
            public Exception Exception { get; set; }
        }
    }
}