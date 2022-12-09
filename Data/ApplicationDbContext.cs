using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyDigitalCv.Helpers;
using MyDigitalCv.Models;

namespace MyDigitalCv.Data
{
	public class ApplicationDbContext : IdentityDbContext<AppUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{

		}
		protected override void ConfigureConventions(ModelConfigurationBuilder builder)
		{
			builder.Properties<DateOnly>()
				.HaveConversion<DateOnlyConverter>()
				.HaveColumnType("date");
		}
		public DbSet<BlogPost> Posts { get; set; }
		public DbSet<AppUser> AppUsers { get; set; }
		public DbSet<Comment> Comments { get; set; }
	}
}
