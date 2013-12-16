namespace Demo.Website.DependencyResolution
{
    using System;
    using System.Web.Mvc;
    using System.Web.Routing;

    using StructureMap;

    public class StructureMapControllerFactory : DefaultControllerFactory
    {
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
                return base.GetControllerInstance(requestContext, null);
            var controller = ObjectFactory.GetInstance(controllerType);
            return (IController)controller;
        }
    }
}
