namespace MyDigitalCv.ViewModels
{
	public class CreatePostViewModel
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Text { get; set; }
		public string? Epigraph { get; set; }
		public DateTime CreationDateTime { get; set; } = DateTime.Now;
		public IFormFile? Image { get; set; }
		public string AppUserId { get; set; }

	}
}
