using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Repositrories.EF_Core.Extensions;

namespace Repositrories.EF_Core
{
	public static class BookRepositoryExtensions
	{
		public static IQueryable<Book> FilterBooks(this IQueryable<Book> books,
			uint minPrice, uint maxPrice) =>
			 books.Where(book => (book.Price >= minPrice) && (book.Price <= maxPrice));

		public static IQueryable<Book> Search(this IQueryable<Book> books,
			string searchTerm)
		{
			if (string.IsNullOrWhiteSpace(searchTerm))
				return books;

			var lowerCaseTerm = searchTerm.Trim().ToLower();
			return books
				.Where(b => b.Title
				.ToLower()
				.Contains(searchTerm));
		}

		public static IQueryable<Book> Sort(this IQueryable<Book> books,
			string orderByQueryStirng)
		{
			if (string.IsNullOrWhiteSpace(orderByQueryStirng))
				return books.OrderBy(b=> b.ID);

			var orderQuery = OrderQueryBuilder
				.CreateOrderQuery<Book>(orderByQueryStirng);

			if (orderQuery is null)
				return books.OrderBy(b=> b.ID);

			return books.OrderBy(orderQuery);
		}
	}
}
