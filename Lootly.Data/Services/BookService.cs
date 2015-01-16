using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lootly.Data.Models;
using NPoco;

namespace Lootly.Data.Services
{
	 public class BookService
	 {
		  public BookService(string connectionStringName)
		  {
				connectionString = connectionStringName;
		  }
		  string connectionString;

		  List<Book> books = new List<Book>() 
				{ 
					 new Book{Id=1, Title="The Wheel of Time" },
					 new Book{Id=2, Title="The Game of Thrones"}
				};

		  public List<Book> GetBooks()
		  {
				IDatabase db = new Database(connectionString);
				List<Book> books = db.Fetch<Book>("select id, title from books");
				return books;
		  }

		  public Book GetBook(int id){
				return books.FirstOrDefault((b) => b.Id == id);
		  }
	 }
}
