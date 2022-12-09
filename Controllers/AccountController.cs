using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyDigitalCv.Data;
using MyDigitalCv.Interfaces;
using MyDigitalCv.Models;
using MyDigitalCv.Services;
using MyDigitalCv.ViewModels;
using System.Security.Cryptography.Xml;

namespace MyDigitalCv.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly IUserRepository _userRepository;
		private readonly IPhotoService _photoService;
		private readonly IHttpContextAccessor _httpContextAccessor;
		public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
			IUserRepository userRepository, IPhotoService photoService, IHttpContextAccessor httpContextAccessor)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_userRepository = userRepository;
			_photoService = photoService;
			_httpContextAccessor = httpContextAccessor;
		}

		public IActionResult Login()
		{
			var response = new LoginViewModel();
			return View(response);
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel loginVM)
		{
			if (!ModelState.IsValid)
			{
				ModelState.AddModelError("", "Unable to log in");
				return View(loginVM);
			}
			var user = await _userRepository.FindByEmailAsync(loginVM.EmailAddress);
			if (user != null)
			{
				//user has been found. Check password
				var passwordChech = await _userManager.CheckPasswordAsync(user, loginVM.Password);
				if (passwordChech)
				{
					//password is correct sign in
					var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
					if (result.Succeeded)
					{
						//var isAuthenticated = User.Identity.IsAuthenticated;
						return RedirectToAction("Index", "Blog");
					}
				}
				//Password is incorrect
				TempData["Error"] = "Wrong credentials.";
				return View(loginVM);
			}
			//user has not been found
			TempData["Error"] = "Wrong credentials.";
			return View(loginVM);
		}
		public IActionResult Register()
		{
			var response = new RegisterViewModel();
			return View(response);
		}
		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel registerVM)
		{
			if (!ModelState.IsValid)
			{
				ModelState.AddModelError("", "Unable to register");
				return View(registerVM);
			}
			var user = await _userManager.FindByEmailAsync(registerVM.EmailAddress);
			if (user != null)
			{
				TempData["Error"] = "This email address is already in use";
				return View(registerVM);
			}
			var newUser = new AppUser
			{
				FirstName = registerVM.FirstName,
				LastName = registerVM.LastName,
				DateOfBirth = registerVM.DateOfBirth,
				Email = registerVM.EmailAddress,
				UserName = registerVM.EmailAddress
			};
			var newUserResponse = await _userManager.CreateAsync(newUser, registerVM.Password);
			if (!newUserResponse.Succeeded)
			{
				TempData["Error"] = "Check password requirements above";
				return View(registerVM);
			}
			await _userManager.AddToRoleAsync(newUser, UserRoles.User);
			return RedirectToAction("Index", "BLog");

		}
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Index", "Blog");
		}

		public async Task<IActionResult> EditProfile(string id)
		{
			var user = await _userRepository.GetUserByIdAsync(id);
			//Creating view model to open 'edit user's profile' page
			var editUserVM = new EditUserViewModel
			{
				Id = user.Id,
				EmailAddress = user.Email,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Phone = user.PhoneNumber,
				Company = user.Company,
				UserName = user.UserName,
				Nickname = user.Nickname,

				//passing profile pic or default link(if user's profile picture is null) to the view model
				ProfilePictureUrl = user.ProfilePictureUrl ?? Data.Const.DefaultUserProfilePicture
			};
			return View(editUserVM);
		}
		[HttpPost]
		public async Task<IActionResult> EditProfile(EditUserViewModel editUserVM)
		{
			if (!ModelState.IsValid)
			{
				ModelState.AddModelError("", "Please fill in all required fields properly");
				return View(editUserVM);
			}
			//finding in the database the user to edit
			var editedUser = await _userRepository.GetUserByIdAsync(editUserVM.Id);
			//if the user has been found changing the fields to the input values
			if (editedUser != null)
			{
				editedUser.FirstName = editUserVM.FirstName;
				editedUser.LastName = editUserVM.LastName;
				editedUser.Phone = editUserVM.Phone;
				editedUser.Company = editUserVM.Company;
				editedUser.ProfilePictureUrl = editUserVM.ProfilePictureUrl;
				editedUser.Nickname = editUserVM.Nickname;
				editedUser.UserName = editUserVM.UserName;
			}
			//user has not been found. Adding the error
			else
			{
				ModelState.AddModelError("", "Couldn't not find the user");
				return View(editUserVM);
			}
			//checking input is not empty and user's previous image was not null
			if (editUserVM.ProfilePicture != null && editUserVM.ProfilePictureUrl != Data.Const.DefaultUserProfilePicture)
			{
				try
				{//deleting the previous pic from the cloud
					await _photoService.DeletePhotoAsync(editUserVM.ProfilePictureUrl);
				}
				catch
				{
					ModelState.AddModelError("", "Unable to delete previous profile picture");
					return View(editUserVM);
				}
			}
			if (editUserVM.ProfilePicture != null)
			{
				//uploading input pic to the cloud
				var profilePictureUploadResult = _photoService.AddPhotoAsync(editUserVM.ProfilePicture);
				//adding the url of the uploaded pic to the edited user
				editedUser.ProfilePictureUrl = profilePictureUploadResult.Result.Url.ToString();
			}
			//updating the user
			_userRepository.Update(editedUser);
			//redirecting to the adited user's profile page
			return RedirectToAction("Detail", "User");
		}
		public IActionResult ChangePassword()
		{
			if (User.Identity.IsAuthenticated)
			{ return View(); }
			else
			{
				ModelState.AddModelError("", "You must sign in to be able to change your password");
				return RedirectToAction("Index", "Blog");
			}
		}
		[HttpPost]
		public async Task<IActionResult> ChangePassword(ChangePasswordViewModel changeVM)
		{
			if (User.Identity.IsAuthenticated)
			{
				var curUserId = _httpContextAccessor.HttpContext.User.GetUserId();
				var user = await _userManager.FindByIdAsync(curUserId);
				if (user != null)
				{
					var checkPassword = await _userManager.CheckPasswordAsync(user, changeVM.OldPassword);
					if (!checkPassword)
					{
						ModelState.AddModelError("", "Old password is incorrect");
						return View(changeVM);
					}
					if (changeVM.NewPassword == changeVM.OldPassword)
					{
						ModelState.AddModelError("", "New password matches the old one.");
						return View(changeVM);
					}
					try
					{
						await _userManager.ChangePasswordAsync(user, changeVM.OldPassword, changeVM.NewPassword);
					}
					catch
					{
						ModelState.AddModelError("", "Unknown error on server side.Sorry");
						return View(changeVM);
					}
				}
				return RedirectToAction("Detail", "User");
			}
			return RedirectToAction("Index", "Blog");
			
		}
		public async Task<IActionResult> ChangeEmail()
		{
			if (User.Identity.IsAuthenticated)
			{
				var curUserId = _httpContextAccessor.HttpContext.User.GetUserId();
				var user = await _userManager.FindByIdAsync(curUserId);
				var changeVm = new ChangeEmailViewModel
				{
					OldEmail = user.Email
				};
				return View(changeVm); 
			}
			else
			{
				ModelState.AddModelError("", "You must sign in to be able to change your email");
				return RedirectToAction("Index", "Blog");
			}
		}
		[HttpPost]
		public async Task<IActionResult> ChangeEmail(ChangeEmailViewModel changeVM)
		{
			if (User.Identity.IsAuthenticated)
			{
				var curUserId = _httpContextAccessor.HttpContext.User.GetUserId();
				var user = await _userManager.FindByIdAsync(curUserId);
				if (user != null)
				{
					if (changeVM.NewEmail == user.Email)
					{
						ModelState.AddModelError("", "New email matches the old one.");
						return View(changeVM);
					}
					if(changeVM.NewEmail == null)
					{
						ModelState.AddModelError("", "You have not entered the new email address");
						return View(changeVM);
					}
					try
					{
						await _userManager.ChangeEmailAsync(user, changeVM.OldEmail, changeVM.NewEmail);
					}
					catch
					{
						ModelState.AddModelError("", "Unknown error on server side.Sorry");
						return View(changeVM);
					}
				}
				return RedirectToAction("Detail", "User");
			}
			return RedirectToAction("Index", "Blog");
		}
	}

}
