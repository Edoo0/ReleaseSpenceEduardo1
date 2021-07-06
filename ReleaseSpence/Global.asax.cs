﻿using System;
using System.Globalization;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ReleaseSpence
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var config = GlobalConfiguration.Configuration;

            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("es-CL");
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
			ModelBinders.Binders.Add(typeof(float), new FloatModelBinder());
			ModelBinders.Binders.Add(typeof(float?), new FloatModelBinder());

            log4net.Config.XmlConfigurator.Configure();
        }
    }
}
