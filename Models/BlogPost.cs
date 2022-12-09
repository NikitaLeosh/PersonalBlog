using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Net.WebRequestMethods;

namespace MyDigitalCv.Models
{
	public class BlogPost
	{
		[Key]
		public int  Id { get; set; }
		public string? ImageUrl { get; set; }
		public string Title { get; set; }
		public string? Epigraph { get; set; }
		public string Text { get; set; }
		public DateTime CreationDateTime { get; set; }
		[ForeignKey("AppUser")]
		public string UserId { get; set; }
	}
}
