using Entities.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositrories.EF_Core.Config
{
	public class BookConfig : IEntityTypeConfiguration<Book>
	{
		public void Configure(EntityTypeBuilder<Book> builder)
		{
			builder.HasData(
				new Book { ID = 1, CategoryId=1, Title = ".net 6 Eğitimi", Price = 100 },
				new Book { ID = 2, CategoryId = 2, Title = "Apı Eğitimi", Price = 400 },
				new Book { ID = 3, CategoryId = 1, Title = "Javascript Eğitimi", Price = 1000 }
				);

		}
	}
}
