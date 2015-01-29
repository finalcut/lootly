using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Lootly.Data;
using Lootly.Data.Services;
using Newtonsoft.Json.Linq;

namespace Lootly.Areas.Api.Controllers
{
	 public abstract class BaseApiController : ApiController
	 {
		  // Needed to rollback transactions on exception
		  public CustomDatabase Database { get; set; }
	 }

	 public abstract class BaseApiController<TService, TModel> : BaseApiController
		  where TService : IService<TModel>
	 {
		  public TService Service { get; set; }
		  //public UserService UserService { get; set; }

		  public virtual IHttpActionResult Get(int id)
		  {
				var obj= Service.Get(id);
				if (obj == null)
				{
					 return NotFound();
				}
				return Ok(obj);
		  }

		  public virtual IEnumerable<TModel> GetAll()
		  {
				return Service.GetAll();
		  }

		  public virtual HttpResponseMessage Post(TModel newItem, string routeName)
		  {
				var id = Service.Create(newItem);
				var response = Request.CreateResponse(HttpStatusCode.Created, Service.Get(id));
				var uri = Url.Link(routeName, new { id });
				response.Headers.Location = new Uri(uri);
				return response;
		  }

		  public virtual TModel Put(object id, JObject data)
		  {
				// https://github.com/schotime/NPoco/wiki/Change-Tracking-for-Updates
				throw new NotImplementedException();
		  }

		  public virtual TModel Patch(object id, JObject data)
		  {
				var poco = data.ToObject<TModel>();
				var propertyNames = data.Properties().Select(p => p.Name);
				Service.Update(id, poco, propertyNames);
				return Service.Get(id);
		  }

		  public virtual int Delete(object id)
		  {
				return Service.Delete(id);
		  }
		  /*
		  protected User CurrentUser()
		  {
				var cookie = Request.Headers.GetCookies("currentUser").FirstOrDefault();
				var username = (cookie != null) ? cookie["currentUser"].Value : Security.GetUsername(User);
				return UserService.GetWhere(u => u.Username == username);
		  }
		   */
	 }

	 public abstract class BaseApiController<TService, TModel, TFilterModel> : BaseApiController<TService, TModel>
		  where TService : IService<TModel, TFilterModel>
		  where TFilterModel : class, new()
	 {
		  public virtual IEnumerable<TModel> Get([FromUri]TFilterModel filter)
		  {
				return Service.GetAll(filter ?? new TFilterModel());
		  }
	 }
}