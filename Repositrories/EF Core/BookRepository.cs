using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using Repositories.EFCore;
using Repositrories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositrories.EF_Core
{
	public sealed class BookRepository : RepositoryBase<Book>, IBookRepository
	{
		public BookRepository(RepositoryContext contex) : base(contex)
		{
			
		}

		public void CreateOneBook(Book book) => Create(book);


		public void DeleteOneBook(Book book) => Delete(book);


		public async Task<PagedList<Book>> GetAllBooksAsync(BookParameters bookParameters, bool trackChanges)
		{
			var books = await FindAll(trackChanges)
			.FilterBooks(bookParameters.MinPrice, bookParameters.MaxPrice)
			.Search(bookParameters.SearchTerm)
			.Sort(bookParameters.OrderBy)
			.ToListAsync();

			return PagedList<Book>
				.ToPagedList(books,
				bookParameters.PageNumber,
				bookParameters.PageSize);
		}

		public async Task<List<Book>> GetAllBooksAsync(bool trackChanges)
		{
			return await FindAll(trackChanges)
				.OrderBy(b=> b.ID)
				.ToListAsync();
		}

		public Task<IEnumerable<Book>> GetAllBooksWithDetailsAsync(bool trackChanges)
		{
			throw new NotImplementedException();
		}

		public async Task<Book> GetOneBookByIdAsync(int id, bool trackChanges) =>
			await FindByCondition(b => b.ID.Equals(id), trackChanges)
			.SingleOrDefaultAsync();
	
		public void UpdateOneBook(Book book) => Update(book);
		
	}
}
