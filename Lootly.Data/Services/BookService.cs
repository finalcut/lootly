using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lootly.Data.Models;
namespace Lootly.Data.Services
{
	 public class BookService
	 {
		  public BookService(string connString)
		  {
				connectionString = connString;
		  }
		  string connectionString;

		  List<Book> books = new List<Book>() 
				{ 
					 new Book{Id=1, Name="The Wheel of Time" },
					 new Book{Id=2, Name="The Game of Thrones"}
				};

		  public List<Book> GetBooks()
		  {
				if (books.Count() < 3)
				{
					 books.Add(new Book { Id = 3, Name = connectionString });
				}
				return books;
		  }

		  public Book GetBook(int id){
				return books.FirstOrDefault((b) => b.Id == id);
		  }
	 }
}
