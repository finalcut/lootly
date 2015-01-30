using System.Collections.Generic;
using System.Web.Http;
using Lootly.Data.Models;
using Lootly.Data.Services;

namespace Lootly.Areas.Api.Controllers
{
	 public class BaseBooksController : BaseApiController<BookService, Book>
    {

		  public override IEnumerable<Book> GetAll()
		  {
				return base.GetAll();
		  }
    }
}
