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
    public class BooksController : ApiController
    {
		  BookService service = new BookService("DefaultConnection");

		  public IEnumerable<Book> GetAllBooks()
		  {
				return service.GetBooks();
		  }

		  public IHttpActionResult GetBook(int id)
		  {
				var book = service.GetBook(id);
				if (book == null)
				{
					 return NotFound();
				}
				return Ok(book);
		  }
    }
}
