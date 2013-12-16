using System;
using System.Configuration;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Demo.Website.Controllers;
using Demo.Website.DependencyResolution;
using StackExchange.Profiling;
using StructureMap;

namespace Demo.Website
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            ObjectFactory.Initialize(i => i.AddRegistry<StructureMapRegistry>());
            ObjectFactory.AssertConfigurationIsValid();
            ControllerBuilder.Current.SetControllerFactory(new StructureMapControllerFactory());

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }


        private void Application_Error(object sender, EventArgs e)
        {
            var customErrorSettings = (CustomErrorsSection)ConfigurationManager.GetSection("system.web/customErrors");
            if (customErrorSettings.Mode == CustomErrorsMode.On)
            {
                var exception = Server.GetLastError();
               Response.Clear();

                var routeData = new RouteData();
                routeData.Values.Add("controller", "Error");
                routeData.Values.Add("action", "Index");
                var httpException = exception as HttpException;
                if (httpException != null)
                {
                    var code = httpException.GetHttpCode();
                    routeData.Values.Add("status", code);
                }
                else
                {
                    routeData.Values.Add("status", 500);
                }

                // Pass exception details to the target error View.
                routeData.Values.Add("error", exception);

                // Clear the error on server.
                Server.ClearError();

                // Avoid IIS getting in the middle
                Response.TrySkipIisCustomErrors = true;

                // Call target Controller and pass the routeData.
                IController errorController = new ErrorController();
                errorController.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
            }
        }

        protected void Application_BeginRequest()
        {
            if (Request.IsLocal)
            {
                MiniProfiler.Start();
                MiniProfiler.Settings.PopupRenderPosition = RenderPosition.Right;
            }
        }

        protected void Application_EndRequest()
        {
            MiniProfiler.Stop();
        }

    }
}