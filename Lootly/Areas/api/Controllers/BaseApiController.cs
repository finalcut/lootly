﻿using System;
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

		  public virtual IHttpActionResult Get(int version, int id)
		  {
				var obj= Service.Get(version, id);
				if (obj == null)
				{
					 return NotFound();
				}
				return Ok(obj);
		  }

		  public virtual IEnumerable<TGetModel> GetAll(int version)
		  {
				return Service.GetAll(version);
		  }

		  public virtual HttpResponseMessage Post(int version, TCreateModel newItem, string routeName)
		  {
				var id = Service.Create(version, newItem);
				var response = Request.CreateResponse(HttpStatusCode.Created, Service.Get(version, id));
				var uri = Url.Link(routeName, new { id });
				response.Headers.Location = new Uri(uri);
				return response;
		  }

		  public virtual TGetModel Put(object id, JObject data)
		  {
				// https://github.com/schotime/NPoco/wiki/Change-Tracking-for-Updates
				throw new NotImplementedException();
		  }

		  public virtual TGetModel Patch(int version, object id, JObject data)
		  {
				var poco = data.ToObject<TUpdateModel>();
				var propertyNames = data.Properties().Select(p => p.Name);
				Service.Update(version, id, poco, propertyNames);
				return Service.Get(version, id);
		  }

		  public virtual int Delete(int version, object id)
		  {
				return Service.Delete(version, id);
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
		  public virtual IEnumerable<TGetModel> Get(int version, [FromUri]TFilterModel filter)
		  {
				return Service.GetAll(version, filter ?? new TFilterModel());
		  }
	 }
}