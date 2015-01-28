using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Lootly;

namespace Lootly
{
	 public class MvcApplication : System.Web.HttpApplication
	 {
		  protected void Application_Start()
		  {
				AreaRegistration.RegisterAllAreas();
				GlobalConfiguration.Configure(WebApiConfig.Register); // webapi has to be first
				RouteConfig.RegisterRoutes(RouteTable.Routes);
		  }
	 }
}
