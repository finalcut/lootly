using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lootly.Data.Models
{
	 [TableName("Books")]
	 public class Book
	 {
		  public int Id { get; set; }
		  public string Title { get; set; }
	 }
}
