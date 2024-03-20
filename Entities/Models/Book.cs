using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
	public class Book
	{
		public int ID { get; set; }
		public string? Title { get; set; }
		public decimal Price { get; set; }

        //ref : navigation property
        public int CategoryId { get; set; }
		public Category Category { get; set; }
	}
}
