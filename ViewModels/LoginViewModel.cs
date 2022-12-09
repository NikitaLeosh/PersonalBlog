using System.ComponentModel.DataAnnotations;

namespace MyDigitalCv.ViewModels
{
	public class LoginViewModel
	{
		[Required(ErrorMessage = "Email is required")]
		[Display(Name = "Email Address")]
		public string EmailAddress { get; set; }
		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }
	
	}
}
