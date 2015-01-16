using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Lootly.Areas.Api.Models;

namespace Lootly.Areas.Api.Controllers
{
    public class BooksController : ApiController
    {
		  Book[] books = new Book[] 
		  { 
				new Book{Id=1, Name="The Wheel of Time"},
				new Book{Id=2, Name="The Game of Thrones"}
		  };

		  public IEnumerable<Book> GetAllBooks()
		  {
				return books;
		  }

		  public IHttpActionResult GetBook(int id)
		  {
				var book = books.FirstOrDefault((b) => b.Id == id);
				if (book == null)
				{
					 return NotFound();
				}
				return Ok(book);
		  }
    }
}
