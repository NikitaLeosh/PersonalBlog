using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyDigitalCv.Models
{
	public class Comment
	{
		[Key]
		public int Id { get; set; }
		public string Text { get; set; }
		[ForeignKey("AppUser")]
		public string UserId { get; set; }
		public string UserFirstName { get; set; }
		public string UserLastName { get; set; }
		public DateTime CreationDateTime { get; set; }
		[ForeignKey("BlogPost")]
		public int PostId { get; set; }

	}
}
