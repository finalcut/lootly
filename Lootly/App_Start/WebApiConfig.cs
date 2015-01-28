using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using Lootly.Data;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using NPoco;
using System.Net.Http.Formatting;
using System.Reflection;

namespace Lootly
{
	 public static class WebApiConfig
	 {
		  public static void Register(HttpConfiguration config)
		  {
				// TODO: Add any additional configuration code.

				// Web API routes
				config.MapHttpAttributeRoutes();

				config.Routes.MapHttpRoute(
					 name: "DefaultApi",
					 routeTemplate: "api/{controller}/{id}",
					 defaults: new { id = RouteParameter.Optional }
				);

				// WebAPI when dealing with JSON & JavaScript!
				// Setup json serialization to serialize classes to camel (std. Json format)
				var formatter = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
				formatter.MediaTypeMappings.Add(new QueryStringMapping("json", "true", "application/json")); // /api/items?json=true
				formatter.SerializerSettings.Converters.Add(new StringEnumConverter());
				formatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
				formatter.Indent = true;

				SetupAutofac(config);
		  }

		  private static void SetupAutofac(HttpConfiguration config)
		  {
				var builder = new ContainerBuilder();


				// Register the Web API controllers.
				builder.RegisterApiControllers(Assembly.GetExecutingAssembly())
					 .PropertiesAutowired();

				var dbFactory = CustomDatabaseFactory.Setup("DefaultConnection");
				builder.Register(c => dbFactory.GetDatabase() as CustomDatabase)
					 .As<IDatabase>()
					 .AsSelf()
					 .InstancePerRequest()
					 .OnActivating(d => d.Instance.BeginTransaction())
					 .OnRelease(d => d.CompleteTransaction());

				builder.RegisterAssemblyTypes(typeof(CustomDatabase).Assembly)
					 .Where(t => t.Name.EndsWith("Service"))
					 .PropertiesAutowired();

				var container = builder.Build();
				config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
		  }
	 }
}