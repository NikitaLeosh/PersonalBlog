using Microsoft.AspNetCore.Identity;
using MyDigitalCv.Models;

namespace MyDigitalCv.Data
{
	public class Seed
	{
		public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
		{
			using(var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
			{
				//Seeding roles
				var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
				if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
					await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
				if (!await roleManager.RoleExistsAsync(UserRoles.User))
					await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

				//Seeding Users
				var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
				var adminUserEmail = "adminuseremail@mail.com";
				var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
				if (adminUser == null)
				{
					var newAdminUser = new AppUser
					{
						UserName="AdminUser",
						FirstName = "Ivan",
						LastName = "Adminovski",
						DateOfBirth = new DateOnly(1990, 2, 15),
						Email = adminUserEmail,
						EmailConfirmed = true
					};
					await userManager.CreateAsync(newAdminUser, "1Admin2Admin345!");
					await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
				}
				var appUserEmail = "appuseremail@mail.com";
				var appUser = await userManager.FindByEmailAsync(appUserEmail);
				if (appUser == null)
				{
					var newAppUser = new AppUser
					{
						UserName = "regularUser",
						FirstName = "Aleksandr",
						LastName = "Polzunski",
						DateOfBirth = new DateOnly(1997, 4, 10),
						Email = appUserEmail,
						EmailConfirmed = true
					};
					await userManager.CreateAsync(newAppUser, "1Admin2Admin345!");
					await userManager.AddToRoleAsync(newAppUser, UserRoles.Admin);
				}
			}
		}
	}
}
