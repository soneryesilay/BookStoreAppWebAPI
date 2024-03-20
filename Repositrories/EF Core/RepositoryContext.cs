using Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Repositrories.EF_Core.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Repositrories.EF_Core
{
	public class RepositoryContext : IdentityDbContext<User>
	{

		public RepositoryContext(DbContextOptions options) :
			base(options)
		{

		}
		public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			//modelBuilder.ApplyConfiguration(new BookConfig());
			//modelBuilder.ApplyConfiguration(new RoleConfiguration());
			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); //tek tek configurasyonları yazmaktansa hepsini burdan tek seferde çektik

		}


	}
}
