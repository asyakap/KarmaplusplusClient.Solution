using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Karmaplusplus.Models;
using Karmaplusplus.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;

namespace Karmaplusplus.Controllers
{
  public class AccountController : Controller
  {
    private readonly KarmaplusplusContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AccountController (UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, KarmaplusplusContext db)
    {
      _userManager = userManager;
      _signInManager = signInManager;
      _db = db;
    }

    [Authorize]
    public async Task<IActionResult> Index()
    { 
      List<Service> userServices = new List<Service> { };
      string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
      using (var httpClient = new HttpClient())
      {
        using (var response = await httpClient.GetAsync($"https://localhost:7225/api/Services?userId={userId}"))
        {
          string apiResponse = await response.Content.ReadAsStringAsync();
          JObject jsonResponse = JObject.Parse(apiResponse);
          JArray serviceArray = (JArray)jsonResponse["data"];
          userServices = serviceArray.ToObject<List<Service>>();
        }
      }
      ViewBag.FirstName = currentUser.FirstName;
      return View(userServices);
    }

    public IActionResult Register()
    {
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Register (RegisterViewModel model)
    {
      if (!ModelState.IsValid)
      {
        return View(model);
      }
      else
      {
        ApplicationUser user = new ApplicationUser { UserName = model.Email , FirstName = model.FirstName };
        IdentityResult result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
          return RedirectToAction("Index");
        }
        else
        {
          foreach (IdentityError error in result.Errors)
          {
            ModelState.AddModelError("", error.Description);
          }
          return View(model);
        }
      }
    }

    public ActionResult Login()
    {
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Login(LoginViewModel model)
    {
      if (!ModelState.IsValid)
      {
        return View(model);
      }
      else
      {
        Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: true, lockoutOnFailure: false);
        if (result.Succeeded)
        {
          return RedirectToAction("Index");
        }
        else
        {
          ModelState.AddModelError("", "There is something wrong with your email or username. Please try again.");
          return View(model);
        }
      }
    }

    [HttpPost]
    public async Task<ActionResult> LogOff()
    {
      await _signInManager.SignOutAsync();
      return RedirectToAction("Index");
    }
  }
}