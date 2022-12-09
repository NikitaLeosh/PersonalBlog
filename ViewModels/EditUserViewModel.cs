using System.ComponentModel.DataAnnotations;

namespace MyDigitalCv.ViewModels
{
	public class EditUserViewModel
	{
		public string Id { get; set; }
		[Display(Name = "Email address")]
		public string EmailAddress { get; set; }
		[Required]
		[Display(Name = "First name")]
		public string FirstName { get; set; }
		[Required]
		[Display(Name = "Last name")]
		public string LastName { get; set; }
		public string? UserName { get; set; }
		public string? Nickname { get; set; }
		[DataType(DataType.Date)]
		[Required]
		[Display(Name = "Date of birth")]
		public DateOnly DateOfBirth { get; set; }
		public string? Phone { get; set; }
		public string? Company { get; set; }
		public IFormFile? ProfilePicture { get; set; }
		public string? ProfilePictureUrl { get; set; }
	}
}
