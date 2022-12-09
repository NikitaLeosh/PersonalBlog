using System.ComponentModel.DataAnnotations;

namespace MyDigitalCv.ViewModels
{
	public class ChangePasswordViewModel
	{
		[Required]
		[Display(Name ="Old password")]
		[DataType(DataType.Password)]
		public string OldPassword { get; set; }
		[Required]
		[Display(Name = "New password")]
		[DataType(DataType.Password)]
		public string NewPassword { get; set; }
		[Required]
		[Display(Name = "Confirm password")]
		[DataType(DataType.Password)]
		[Compare("NewPassword")]
		public string ConfirmPassword { get; set; }
	}
}
