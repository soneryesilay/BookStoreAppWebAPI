using Entities.Models;
using Repositrories.EF_Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
	public interface ICategoryRepository : IRepositoryBase<Category>
	{
		Task<IEnumerable<Category>> GetAllCategoriesAsync(bool trackChanges);
		Task<Category> GetOneCategoryByIdAsync(int id, bool trackChanges);
		void CreateOneCategory(Category category);
		void UpdateOneCategory(Category category);
		void DeleteOneCategory(Category category);

		Task<IEnumerable<Category>> GetAllBooksWithDetailsAsync(bool trackChanges);

	}
}