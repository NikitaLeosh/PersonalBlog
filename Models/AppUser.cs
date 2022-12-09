using Microsoft.AspNetCore.Identity;

namespace MyDigitalCv.Models
{
	public class AppUser : IdentityUser
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string? Nickname { get; set; }
		public DateOnly DateOfBirth { get; set; }
		public string? Phone { get; set; }
		public string? Company { get; set; }
		public string? ProfilePictureUrl { get; set; }
	}
}
