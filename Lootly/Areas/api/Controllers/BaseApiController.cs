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

	 public abstract class BaseApiController<TService, TGetModel, TCreateModel, TUpdateModel> : BaseApiController
		  where TService : IService<TGetModel, TCreateModel, TUpdateModel>
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

		  public virtual IEnumerable<TGetModel> GetAll()
		  {
				return Service.GetAll();
		  }

		  public virtual HttpResponseMessage Post(TCreateModel newItem, string routeName)
		  {
				var id = Service.Create(newItem);
				var response = Request.CreateResponse(HttpStatusCode.Created, Service.Get(id));
				var uri = Url.Link(routeName, new { id });
				response.Headers.Location = new Uri(uri);
				return response;
		  }

		  public virtual TGetModel Put(object id, JObject data)
		  {
				// https://github.com/schotime/NPoco/wiki/Change-Tracking-for-Updates
				throw new NotImplementedException();
		  }

		  public virtual TGetModel Patch(object id, JObject data)
		  {
				var poco = data.ToObject<TUpdateModel>();
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

	 public abstract class BaseApiController<TService, TGetModel, TCreateModel, TUpdateModel, TFilterModel> : BaseApiController<TService, TGetModel, TCreateModel, TUpdateModel>
		  where TService : IService<TGetModel, TCreateModel, TUpdateModel, TFilterModel>
		  where TFilterModel : class, new()
	 {
		  public virtual IEnumerable<TGetModel> Get([FromUri]TFilterModel filter)
		  {
				return Service.GetAll(filter ?? new TFilterModel());
		  }
	 }
}