using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Lootly.Data.Models;
using Lootly.Data.Services;

namespace Lootly.Areas.Api.Controllers
{
	 public class BaseBooksController : BaseApiController<BookService, Book, Book, Book>
    {

		  public override IEnumerable<Book> GetAll(int version)
		  {
				return base.GetAll(version);
		  }

		  public override IHttpActionResult Get(int version, int id)
		  {
				return base.Get(version, id);
		  }
    }
}
