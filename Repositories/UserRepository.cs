using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MyDigitalCv.Data;
using MyDigitalCv.Interfaces;
using MyDigitalCv.Models;
using MyDigitalCv.ViewModels;

namespace MyDigitalCv.Repositories
{
	public class UserRepository : IUserRepository
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;


		public UserRepository(ApplicationDbContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
		{
			_context = context;
			_userManager = userManager;
			_signInManager = signInManager;
		}
		public async Task<bool> Add(RegisterViewModel registerVM)
		{
			var newUser = new AppUser
			{
				FirstName = registerVM.FirstName,
				LastName = registerVM.LastName,
				DateOfBirth = registerVM.DateOfBirth,
				Email = registerVM.EmailAddress,
				UserName = registerVM.EmailAddress
			};
			var newUserResponse = await _userManager.CreateAsync(newUser, registerVM.Password);
			if (!newUserResponse.Succeeded)
			{
				return false;
			}
			await _userManager.AddToRoleAsync(newUser, UserRoles.User);
			return true;
		}

		public bool Delete(AppUser user)
		{
			_context.Remove(user);
			return true;
		}

		public async Task<AppUser> FindByEmailAsync(string email)
		{
			return await _userManager.FindByEmailAsync(email);
		}

		public bool Save()
		{
			var result = _context.SaveChanges();
			return (result > 0) ? true : false;
		}

		public  bool Update(AppUser user)
		{
			_context.Update(user);
			return Save();
		}
		public IEnumerable<AppUser> GetAll()
		{
			return _context.AppUsers;
		}
		public async Task<AppUser> GetUserByIdAsync(string id)
		{
			return await _context.AppUsers.FindAsync(id);
		}
	}
}
