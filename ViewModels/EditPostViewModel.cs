namespace MyDigitalCv.ViewModels
{
	public class EditPostViewModel
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Text { get; set; }
		public string? Epigraph { get; set; }
		public DateTime CreationDateTime { get; set; }
		public IFormFile? Image { get; set; }
		public string? ImageUrl { get; set; }
	}
}
