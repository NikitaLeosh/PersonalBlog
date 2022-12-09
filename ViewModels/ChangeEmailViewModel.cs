using System.ComponentModel.DataAnnotations;

namespace MyDigitalCv.ViewModels
{
	public class ChangeEmailViewModel
	{
		[Display(Name = "Old email")]
		public string OldEmail { get; set; }
		[Display(Name = "New email")]
		[Required(ErrorMessage = "You should fill in this field")]

		public string NewEmail { get; set; }
	}
}
