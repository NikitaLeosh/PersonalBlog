using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Metadata;
using MyDigitalCv.Data;
using MyDigitalCv.Interfaces;
using MyDigitalCv.Models;
using MyDigitalCv.ViewModels;
using System.Diagnostics;
using System.Reflection;
using System.Xml.Linq;

namespace MyDigitalCv.Controllers
{
	public class BlogController : Controller
	{
		private readonly ILogger<BlogController> _logger;
		private readonly IBlogPostRepository _postRepository;
		private readonly IPhotoService _photoService;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly ICommentsRepository _commentsRepository;
		private readonly IUserRepository _userRepository;

		public BlogController(ILogger<BlogController> logger, IBlogPostRepository postRepository, IUserRepository userRepository,
			IPhotoService photoService, IHttpContextAccessor httpContextAccessor, ICommentsRepository commentsRepository)
		{
			_postRepository = postRepository;
			_logger = logger;
			_photoService = photoService;
			_httpContextAccessor = httpContextAccessor;
			_commentsRepository = commentsRepository;
			_userRepository = userRepository;
		}

		public IActionResult Index()
		{
			IEnumerable<BlogPost> posts = _postRepository.GetAll();
			return View(posts);
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
		public IActionResult CreatePost()
		{
			var curUserId = _httpContextAccessor.HttpContext.User.GetUserId();
			var createPostVM = new CreatePostViewModel()
			{
				AppUserId = curUserId
			};
			return View(createPostVM);
		}
		[HttpPost]
		public async Task<IActionResult> CreatePost(CreatePostViewModel postVM)
		{
			if (ModelState.IsValid)
			{
				var imageUploadResult = await _photoService.AddPhotoAsync(postVM.Image);

				BlogPost post = new BlogPost
				{
					Id = postVM.Id,
					Title = postVM.Title,
					Text = postVM.Text,
					CreationDateTime = postVM.CreationDateTime,
					ImageUrl = imageUploadResult.Url.ToString(),
					UserId = postVM.AppUserId
				};
				_postRepository.Add(post);

			}
			else return View("Error");
			return RedirectToAction("Index");

		}
		public async Task<IActionResult> PostDetails(int id)
		{
			var post = await _postRepository.GetByIdAsync(id);
			var postVM = new PostDetailsViewModel
			{
				Id = post.Id,
				Title = post.Title,
				Text = post.Text,
				CreationDateTime = post.CreationDateTime,
				ImageUrl = post.ImageUrl,
				Epigraph = post.Epigraph,
				UserId = post.UserId,
			};
			postVM.Comments = (await _commentsRepository.GetAllByPostId(post.Id)).OrderByDescending(n => n.Id);
			return View(postVM);
		}
		public async Task<IActionResult> EditPost(int id)
		{
			var post = await _postRepository.GetByIdAsync(id);
			if (post == null)
			{
				ModelState.AddModelError("UserError", "failed to find the post");
				return View("Error", ModelState);
			}
			var postVM = new EditPostViewModel
			{
				Id = post.Id,
				Title = post.Title,
				Text = post.Text,
				CreationDateTime = post.CreationDateTime,
				ImageUrl = post.ImageUrl
			};

			return View(postVM);
		}
		[HttpPost]
		public async Task<IActionResult> EditPost(EditPostViewModel postVM)
		{

			if (!ModelState.IsValid)
			{
				ModelState.AddModelError("UserError", "Failed to edit this post");
				return View(postVM);
			}
			var postBeingEdited = await _postRepository.GetByIdAsync(postVM.Id);
			if (postBeingEdited != null)
			{
				postBeingEdited.Title = postVM.Title;
				postBeingEdited.Text = postVM.Text;
				postBeingEdited.CreationDateTime = DateTime.Now;
				if (postVM.Image != null)
				{

					try
					{
						await _photoService.DeletePhotoAsync(postBeingEdited.ImageUrl);
					}
					catch
					{
						ModelState.AddModelError("ImageUrl", "Failed to delete the image");
						return View(postVM);
					}
					var uploadImageResult = await _photoService.AddPhotoAsync(postVM.Image);
					if (uploadImageResult == null)
					{
						ModelState.AddModelError("ImageUrl", "Failed to upload the image");
						return View(postVM);
					}
					postBeingEdited.ImageUrl = uploadImageResult.Url.ToString();
				}
			}
			else
			{
				ModelState.AddModelError("UserError", "Failed to edit this post");
				return View(postVM);
			}
			_postRepository.Update(postBeingEdited);
			return RedirectToAction("Index");
		}
		public async Task<IActionResult> DeletePost(int id)
		{
			var postToDelete = await _postRepository.GetByIdAsync(id);
			var deletePostVM = new DeletePostViewModel
			{
				Id = postToDelete.Id,
				Title = postToDelete.Title,
				ImageUrl = postToDelete.ImageUrl,
				CreationDateTime = postToDelete.CreationDateTime
			};
			return View(deletePostVM);
		}
		[HttpPost]
		public async Task<IActionResult> DeletePost(DeletePostViewModel deletePostVM)
		{
			var postToDelete = await _postRepository.GetByIdAsync(deletePostVM.Id);
			try
			{
				_photoService.DeletePhotoAsync(postToDelete.ImageUrl);
			}
			catch
			{
				ModelState.AddModelError("UserError", "Failed to delete the image");
				return View(postToDelete);
			}
			_postRepository.Delete(postToDelete);
			return RedirectToAction("Index");
		}
		[HttpPost]
		public async Task<IActionResult> AddComment(PostDetailsViewModel postDetailVM)
		{
			var curUserId = _httpContextAccessor.HttpContext.User.GetUserId();
			var user = await _userRepository.GetUserByIdAsync(curUserId);
			var comment = new Comment
			{
				PostId = postDetailVM.Id,
				Text = postDetailVM.NewComment.Text,
				CreationDateTime = DateTime.Now,
				UserId = curUserId,
				UserFirstName = user.FirstName,
				UserLastName = user.LastName
			};
			_commentsRepository.Add(comment);
			return RedirectToAction("PostDetails", 
				new RouteValueDictionary(new { Controller = "Blog", Action = "PostDetails", Id = postDetailVM.Id }));
		}
	}
} 