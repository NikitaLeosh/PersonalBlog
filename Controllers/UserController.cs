using Microsoft.AspNetCore.Mvc;
using MyDigitalCv.Interfaces;
using MyDigitalCv.Models;
using MyDigitalCv.ViewModels;
using static System.Net.WebRequestMethods;

namespace MyDigitalCv.Controllers
{
	public class UserController : Controller
	{
		private readonly IUserRepository _userRepository;
		private readonly IPhotoService _photoService;
		private readonly IHttpContextAccessor _httpAccessor;
		public UserController(IUserRepository userRepository, IPhotoService photoService, IHttpContextAccessor httpAccessor)
		{
			_userRepository = userRepository;
			_photoService = photoService;
			_httpAccessor = httpAccessor;
		}
		public IActionResult Index()
		{
			var users = _userRepository.GetAll();
			return View(users);
		}
		public async Task<IActionResult> Detail()
		{
			var curUserId = _httpAccessor.HttpContext.User.GetUserId();
			var user = await _userRepository.GetUserByIdAsync(curUserId);		
			return View(user);
		}
		
	}
}
