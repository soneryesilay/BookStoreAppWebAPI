﻿using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositrories.EF_Core.Config
{
	public class CategoryConfig : IEntityTypeConfiguration<Category>
	{
		public void Configure(EntityTypeBuilder<Category> builder)
		{
			builder.HasKey(c => c.CategoryId);
			builder.Property(c => c.CategoryId).IsRequired();

			builder.HasData(
				new Category()
				{
					CategoryId = 1,
					CategoryName = "Computer Science"
				},
				new Category()
				{
					CategoryId = 2,
					CategoryName = "Network Science"
				},
				new Category()
				{
					CategoryId = 3,
					CategoryName = "Database Managment"
				}
			   );
		}
	}
}
