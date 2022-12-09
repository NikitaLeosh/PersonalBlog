using System.ComponentModel.DataAnnotations;

namespace MyDigitalCv.ViewModels
{
	public class RegisterViewModel
	{
		[Display(Name = "Email address")]
		[Required(ErrorMessage = "Email address is required")]
		public string EmailAddress { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Display(Name = "Confirm password")]
		[Required(ErrorMessage = "This is a required field")]
		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "Password shoud match the \"Confirm password\" field")]
		public string ConfirmPassword { get; set; }
		[Required]
		[Display(Name = "First name")]
		public string FirstName { get; set; }
		[Required]
		[Display(Name = "Last name")]
		public string LastName { get; set; }
		[DataType(DataType.Date)]
		[Required]
		[Display(Name = "Date of birth")]
		public DateOnly DateOfBirth { get; set; }
	}
}
