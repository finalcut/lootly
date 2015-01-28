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
    public class BooksController : BaseApiController<BookService, Book, Book, Book>
    {

		  public override IEnumerable<Book> GetAll()
		  {
				return base.GetAll();
		  }

		  public override IHttpActionResult Get(int id)
		  {
				return base.Get(id);
		  }
    }
}
