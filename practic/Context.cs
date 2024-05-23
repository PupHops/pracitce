using Microsoft.EntityFrameworkCore;

using practic.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace practic
{
	public class Context: DbContext
	{
		//public readonly string connectionString = "Initial Catalog=bibaNew;Server=192.168.221.12;user=user04;password=04;TrustServerCertificate=true";

		//private readonly string connectionString = "Data Source=DESKTOP-HIITB3O; initial Catalog=biba; " +
		//"Integrated Security=True;TrustServerCertificate=True";
		private readonly string connectionString = "Data Source=DESKTOP-SD2NSU5\\MSSQLSERVER05; initial Catalog=zalupa; " +
"Integrated Security=True;TrustServerCertificate=True";
		public DbSet<Activity> Activities { get; set; }
		public DbSet<Event> Events { get; set; }
		public DbSet<ActivityEvent> ActivityEvents { get; set; }
		public DbSet<City> Cities { get; set; }
		public DbSet<Country> Countries { get; set; }
		public DbSet<Course> Course { get; set; }
		public DbSet<Role> Roles { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<ActivityJury> ActivityJuries { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(connectionString);
		}


		public Context()
		{
			//Database.EnsureDeleted();
			//Database.EnsureCreated();
		}
	}
}
