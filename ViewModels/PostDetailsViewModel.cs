using MyDigitalCv.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyDigitalCv.ViewModels
{
	public class PostDetailsViewModel
	{
		public int Id { get; set; }
		public string? ImageUrl { get; set; }
		public string Title { get; set; }
		public string? Epigraph { get; set; }
		public string Text { get; set; }
		public DateTime CreationDateTime { get; set; }
		[ForeignKey("AppUser")]
		public string UserId { get; set; }
		public IEnumerable<Comment>? Comments { get; set; }
		public Comment? NewComment { get; set; }
	}
}
