using Microsoft.AspNetCore.Mvc;
using Karmaplusplus.Models;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace Karmaplusplus.Controllers;

[Authorize]
public class VolunteeringsController : Controller
{
  private readonly KarmaplusplusContext _db;
  private readonly UserManager<ApplicationUser> _userManager;
  public VolunteeringsController(UserManager<ApplicationUser> userManager, KarmaplusplusContext db)
  {
    _userManager = userManager;
    _db = db;
  }

  public async Task<IActionResult> Index( int page = 1, int pageSize = 6)
  {
    Volunteering volunteering = new Volunteering();
    List<Volunteering> volunteeringList = new List<Volunteering> { };
    using (var httpClient = new HttpClient())
    {
      using (var response = await httpClient.GetAsync($"https://localhost:7226/api/Volunteerings?page={page}&pageSize={pageSize}"))
      {
        string apiResponse = await response.Content.ReadAsStringAsync();
        JObject jsonResponse = JObject.Parse(apiResponse);
        JArray volunteeringArray = (JArray)jsonResponse["data"];
        volunteeringList = volunteeringArray.ToObject<List<Volunteering>>();
      }
    }

    List<Volunteering> totalVolunteerings = new List<Volunteering> { };
    using (var httpClient = new HttpClient())
    {
      using (var response = await httpClient.GetAsync($"https://localhost:7226/api/Volunteerings?page={1}&pageSize=1001"))
      {
        string apiResponse = await response.Content.ReadAsStringAsync();
        JObject jsonResponse = JObject.Parse(apiResponse);
        JArray volunteeringArray = (JArray)jsonResponse["data"];
        totalVolunteerings = volunteeringArray.ToObject<List<Volunteering>>();
      }
    }
    ViewBag.Remainder = totalVolunteerings.Count() % 6;
    ViewBag.TotalPages = (totalVolunteerings.Count() / 6);
    ViewBag.CurrentPage = page;
    ViewBag.PageSize = pageSize;
    
    return View(volunteeringList);
  }

  public async Task<IActionResult> Details(int id)
  {
    List<Volunteering> VolunteeringList = new List<Volunteering> { };
    using (var httpClient = new HttpClient())
    {
      using (var response = await httpClient.GetAsync($"https://localhost:7226/api/Volunteerings?id={id}"))
      {
        string apiResponse = await response.Content.ReadAsStringAsync();
        JObject jsonResponse = JObject.Parse(apiResponse);
        JArray volunteeringArray = (JArray)jsonResponse["data"];
        VolunteeringList = volunteeringArray.ToObject<List<Volunteering>>();
      }
    }
    string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    ViewBag.UserId = userId;
    Volunteering volunteering = VolunteeringList[0];
    return View(volunteering);
  }

  public ActionResult Create()
  {
    return View();
  }

  [HttpPost]
  public async Task<IActionResult> Create(Volunteering volunteering)
  {
    if(!ModelState.IsValid)
    {
      return View(volunteering);
    }
    string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
    volunteering.UserId = userId;
    Volunteering.PostVolunteering(volunteering);
    return RedirectToAction("Index");
  }

  public async Task<IActionResult> Edit(int id)
  {
    string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
    List<Volunteering> VolunteeringList = new List<Volunteering> { };
    using (var httpClient = new HttpClient())
    {
      using (var response = await httpClient.GetAsync($"https://localhost:7226/api/Volunteerings?id={id}"))
      {
        string apiResponse = await response.Content.ReadAsStringAsync();
        JObject jsonResponse = JObject.Parse(apiResponse);
        JArray volunteeringArray = (JArray)jsonResponse["data"];
        VolunteeringList = volunteeringArray.ToObject<List<Volunteering>>();
      }
    }
    Volunteering volunteering = VolunteeringList[0];
    if(volunteering.UserId != userId)
    {
      Error error = new Error { ErrorMessage = "You can only edit or delete your own posts!" };
      return RedirectToAction( "Error", error );
    }
    else
    {
      return View(volunteering);
    }
  }

  [HttpPost]
  public ActionResult Edit(Volunteering volunteering)
  {
    Volunteering.PutVolunteering(volunteering);
    return RedirectToAction("Details", new { id = volunteering.VolunteeringId });
  }

  public ActionResult Delete(int id)
  {
    string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    Volunteering volunteering = Volunteering.GetVolunteeringDetails(id);
    if(volunteering.UserId != userId)
    {
      Error error = new Error { ErrorMessage = "You can only edit or delete your own posts!" };
      return RedirectToAction("Error", error);
    }
    else
    {
      return View(volunteering);
    }
  }


  [HttpPost, ActionName("Delete")]
  public ActionResult DeleteConfirmed(int id)
  {
    Volunteering.DeleteVolunteering(id);
    return RedirectToAction("Index");
  }

  [HttpGet]
  public ActionResult Error(Error error)
  {
    return View(error);
  }

  [HttpPost, ActionName("Search")]
  public async Task<IActionResult> Search(string name)
  {
    if(name == null)
    {
      return RedirectToAction("Index");
    }
    List<Volunteering> VolunteeringList = new List<Volunteering> { };
    using (var httpClient = new HttpClient())
    {
      using (var response = await httpClient.GetAsync($"https://localhost:7226/api/Volunteerings?pageSize=1001"))
      {
        string apiResponse = await response.Content.ReadAsStringAsync();
        JObject jsonResponse = JObject.Parse(apiResponse);
        JArray volunteeringArray = (JArray)jsonResponse["data"];
        VolunteeringList = volunteeringArray.ToObject<List<Volunteering>>();
      }
    }
    List<Volunteering> result = new List<Volunteering> { };
    foreach(Volunteering volunteering in VolunteeringList)
    {
      if (volunteering.Description.ToLower().Contains(name.ToLower()))
      {
        result.Add(volunteering);
      }
    }
    ViewBag.SearchResults = name;
    return View(result);
  }
}
