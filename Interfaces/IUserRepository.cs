using MyDigitalCv.Models;
using MyDigitalCv.ViewModels;

namespace MyDigitalCv.Interfaces
{
	public interface IUserRepository
	{
		Task<bool> Add(RegisterViewModel registerViewModel);
		bool Update(AppUser user);
		bool Delete(AppUser user);
		bool Save();
		Task<AppUser> FindByEmailAsync(string email);
		Task<AppUser> GetUserByIdAsync(string id);
		IEnumerable<AppUser> GetAll();

	}
}
